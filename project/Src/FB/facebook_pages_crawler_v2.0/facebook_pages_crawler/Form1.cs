using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Collections;
using System.IO;

namespace facebook_pages_crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.IsBackground = true;
            Form.CheckForIllegalCrossThreadCalls = false;
        }
        bool[] resultBool = new bool[4];
        List<string> dbInfo = new List<string>();
        Thread updatePageThread = null;
        int maxLimit = 500;
        int likeLimit = 1000;
        int commentLimit = 2000;
        bool updatingUser = false;
        int timerInterval = 60 * 60 * 1000;//一小時
        Thread startUpdateUserInfo = null;
        string dbInfoFileName = @"fbcrawler.data";
        private void dbConnect_Click(object sender, EventArgs e)
        {
            if (dbServerBox.Text.Length * dbAcoountBox.Text.Length * dbPasswordBox.Text.Length * dbTableBox.Text.Length == 0)
            {
                MessageBox.Show("資料庫資訊不得為空！");
                return;
            }
            dbInfo.Clear();
            dbInfo.Add(dbServerBox.Text);
            dbInfo.Add(dbAcoountBox.Text);
            dbInfo.Add(dbPasswordBox.Text);
            dbInfo.Add(dbTableBox.Text);
            string errorMessage = "";
            DBConnection conn = new DBConnection(dbInfo);
            if (!conn.checkDBExist())
            {
                MessageBox.Show("資料庫連線資訊錯誤!!");
                return;
            }
            errorMessage = conn.MySqlConnect();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            else
            {
                accessTokenGroup.Enabled = true;
                string sql = "select count(*) from access_token where 1";
                MySqlDataReader res = conn.mysql_query(sql);
                res.Read();
                if (res.GetInt32(0) > 0)
                {
                    res.Close();
                    sql = "select access_token from access_token where 1";
                    res = conn.mysql_query(sql);
                    res.Read();
                    accessTokenBox.Text = res.GetString(0);
                    pagesEditGroup.Enabled = true;
                    res.Close();
                    checkCanUpdate();
                    init_Timer();
                }
                else
                {
                    res.Close();
                    MessageBox.Show("請先指定Access Token");
                }
            }
            FileStream stream = new FileStream(dbInfoFileName, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(stream);
            string baseServer = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(dbServerBox.Text));
            string baseAccount = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(dbAcoountBox.Text));
            string basePWD = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(dbPasswordBox.Text));
            string baseTable = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(dbTableBox.Text));
            writer.WriteLine(baseServer + ":" + baseAccount + ":" + basePWD + ":" + baseTable);
            writer.Close();
            stream.Close();
            updatePageIDList();
            conn.closeMySqlConnection();
            dbConnInfo.Enabled = false;
        }

        private JSONObject resultCheck(JSONObject result, string url, int limit)
        {
            FBGraph api = new FBGraph();
            int erro_count = 0;
            int sleepTime = 30 * 1000;
            int retryTIme = 10 * 1000;
            while (true)
            {
                if (result != null && result.IsDictionary == false) break;
                if (result == null || result.Dictionary.ContainsKey("error_msg") || result.Dictionary.ContainsKey("error"))
                {
                    //若發生error 檢查是何種error 若無error再透過exception離開迴圈
                    string errorMessage = "";
                    string errorNumber = "";
                    if (result == null)
                    {
                        errorNumber = "1";
                    }
                    else if (result.Dictionary.ContainsKey("error_msg"))
                    {
                        errorMessage = result.Dictionary["error_msg"].String;
                        errorNumber = result.Dictionary["error_code"].String;
                    }
                    else
                    {
                        errorMessage = result.Dictionary["error"].Dictionary["message"].String;
                        errorNumber = result.Dictionary["error"].Dictionary["code"].String;
                    }

                    switch (errorNumber)
                    {
                        case "1":
                        case "2":
                            if (limit <= 25)
                            {
                                messageLog(errorMessage);
                                return null;
                            }
                            limit = limit / 2;
                            if (limit > 0)
                            {
                                url += "&limit=" + limit.ToString();
                            }
                            Thread.Sleep(retryTIme);
                            break;
                        case "4":
                            messageLog(errorMessage);
                            erro_count++;
                            if (erro_count > 30)
                            {   
                                return null;
                            }
                            Thread.Sleep(sleepTime);
                            break;
                        case "602":
                            messageLog(errorMessage);
                            return null;
                    }
                    result = api.FBGraphGet(url);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private void init_Timer()
        {
            return;
            if (userInfoTimer.Enabled)
            {
                userInfoTimer.Stop();
                userInfoTimer.Enabled = false;
                userInfoTimer.Dispose();
                updatingUser = false;
                if (startUpdateUserInfo != null)
                {
                    startUpdateUserInfo.Abort();
                    startUpdateUserInfo = null;
                }
            }
            userInfoTimer.Interval = timerInterval;
            userInfoTimer.Enabled = true;
            userInfoTimer.Start();
            startUpdateUserInfo = new Thread(delegate() { update_user_info(); });
            startUpdateUserInfo.Start();
        }

        private void update_user_info()
        {
            if (updatingUser)
                return;
            updatingUser = true;
            DateTime start = DateTime.Now;
            Console.WriteLine("啟動更新@" + DateTime.Now.ToString("HH:mm:ss") + "!!!");
            if (accessTokenBox.Text.Length >= 0)
            {
                string sql = "select id from user_info where username = \"\" order by updatetime asc";
                DBConnection conn = new DBConnection(dbInfo);
                conn.MySqlConnect();
                List<string> userID = new List<string>();
                MySqlDataReader res = conn.mysql_query(sql);
                while (res.Read())
                {
                    userID.Add(res.GetString(0));
                }
                res.Close();
                StringBuilder sb = new StringBuilder();
                int index = 0;
                foreach (string uid in userID)
                {
                    if (index % 1500 == 0)
                    {
                        if (index != 0)
                        {
                            update_user_info_by_ids(sb, conn);
                        }
                        sb.Clear();
                        sb.Append(uid);
                    }
                    else
                    {
                        sb.Append("," + uid);
                    }
                    index++;
                }
                update_user_info_by_ids(sb, conn);
                conn.closeMySqlConnection();
            }
            updatingUser = false;

            Console.WriteLine("更新結束@" + DateTime.Now.ToString("HH:mm:ss") + "!!!(" + DateTime.Now.Subtract(start).TotalMinutes.ToString(".00Mins"));
        }
        private JSONObject FBJsonCheck(JSONObject result)
        {

            return null;
        }
        private void update_user_info_by_ids(StringBuilder sb, DBConnection conn)
        {
            Thread.Sleep(5 * 1000);
            string sql = "";
            string url = "https://graph.facebook.com/fql?q=select uid,username,sex from user where uid in (" + sb.ToString() + ")&access_token=" + accessTokenBox.Text;
            //showMessage.AppendText(url);
            //return;
            FBGraph api = new FBGraph();
            JSONObject result = api.FBGraphGet(url);
            result = resultCheck(result, url, 0);
            if (result == null)
            {
                return;
            }
            //try
            //{
            //    int erro_count = 0;
            //    while (true)
            //    {
            //        if (result.Dictionary.ContainsKey("error_msg") || result.Dictionary.ContainsKey("error"))
            //        {
            //            //若發生error 檢查是何種error 若無error再透過exception離開迴圈
            //            string errorMessage = "";
            //            string errorNumber = "";
            //            if (result.Dictionary.ContainsKey("error_msg"))
            //            {
            //                errorMessage = result.Dictionary["error_msg"].String;
            //                errorNumber = result.Dictionary["error_code"].String;
            //            }
            //            else
            //            {
            //                errorMessage = result.Dictionary["error"].Dictionary["message"].String;
            //                errorNumber = result.Dictionary["error"].Dictionary["code"].String;
            //            }
            //            //Console.WriteLine("Error message(" + errorNumber + ") is : " + errorMessage);
            //            if (errorNumber.Equals("602"))
            //            {
            //                messageLog(errorMessage);
            //                //showMessage.AppendText(errorMessage + "\n");
            //                return;
            //            }
            //            else
            //            {
            //                Thread.Sleep(1000);
            //                erro_count++;
            //                if (erro_count > 30)
            //                {
            //                    messageLog(errorMessage);
            //                    //showMessage.AppendText(errorMessage + "\n");
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            //catch { }
            sb.Clear();
            Dictionary<string, string[]> uInfo = new Dictionary<string, string[]>();
            if (result.Dictionary.ContainsKey("data") && result.Dictionary["data"].Array.Length > 0)
            {
                foreach (JSONObject pageData in result.Dictionary["data"].Array)
                {
                    string[] tmpString = { pageData.Dictionary["username"].String, pageData.Dictionary["sex"].String };
                    uInfo.Add(pageData.Dictionary["uid"].String, tmpString);
                    sb.Append(pageData.Dictionary["uid"].String + ",");
                }
                string allIDs = sb.ToString();
                allIDs = allIDs.Remove(allIDs.Length - 1);
                sb.Clear();
                sb.Append("update user_info set username = case id ");
                foreach (KeyValuePair<string, string[]> kvp in uInfo)
                {
                    sb.Append(" when " + kvp.Key + " then '" + kvp.Value[0] + "' ");
                }
                sb.Append(" end, gender = case id ");
                foreach (KeyValuePair<string, string[]> kvp in uInfo)
                {
                    sb.Append(" when " + kvp.Key + " then '" + kvp.Value[1] + "' ");
                }
                sb.Append(" end where id in (" + allIDs + ")");
                sql = sb.ToString();
                conn.mysql_query(sql).Close();
                sql = "update user_info set updatetime = ?updatetime where id in (" + allIDs + ")";
                MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
                cmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                cmd.ExecuteReader().Close();
                cmd.Parameters.Clear();
            }
        }

        private DateTime getCurrentDate()
        {
            string url = "http://www.timeanddate.com/worldclock/city.html?n=241";
            FBGraph api = new FBGraph();
            string result = api.result(url);
            string pattern = "Current Time</th><td><strong id=ct  class=big>(?<item>[^<]+)</strong>";
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(result, pattern);
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    string timeString = matches[i].Groups["item"].Value;
                    string[] partString = timeString.Split(',');
                    try
                    {
                        return Convert.ToDateTime(partString[1].Trim());
                    }
                    catch { }
                }
            }
            return new DateTime(2011, 1, 1);
        }

        public void checkCanUpdate()
        {
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "select count(*) from pages_info where 1";
            MySqlDataReader res = conn.mysql_query(sql);
            res.Read();
            if (res.GetInt32(0) > 0)
            {
                startUpdate.Enabled = true;
            }
            else
            {
                startUpdate.Enabled = false;
            }
            res.Close();
            conn.closeMySqlConnection();
        }
        private void SetAccessToken_Click(object sender, EventArgs e)
        {
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "truncate table access_token";
            conn.mysql_query(sql).Close();

            sql = "insert into access_token (access_token) values (\"" + accessTokenBox.Text + "\")";
            conn.mysql_query(sql).Close();
            pagesEditGroup.Enabled = true;
            conn.closeMySqlConnection();
            init_Timer();
        }

        private void insertPageID_Click(object sender, EventArgs e)
        {
            if (searchResultList.Items.Count == 0)
            {
                MessageBox.Show("請先搜尋粉絲頁");
                return;
            }
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "";
            string IDString = "";
            int insertCount = 0;
            foreach (ListViewItem lvi in searchResultList.Items)
            {
                if (!lvi.Checked)
                    continue;
                IDString += lvi.Text + ",";
                insertCount++;
            }
            if (insertCount == 0)
            {
                MessageBox.Show("請先勾選要新增的粉絲頁名單");
                return;
            }
            IDString = IDString.Substring(0, IDString.Length - 1);
            string url = "https://api.facebook.com/method/fql.query?query=select page_id,name,pic_small,page_url,website,founded,fan_count,general_info,type from page where page_id in (" + IDString + ")&access_token=" + accessTokenBox.Text + "&format=json";
            FBGraph api = new FBGraph();
            JSONObject result = api.FBGraphGet(url);
            result = resultCheck(result, url, 0);
            //try
            //{
            //    int erro_count = 0;
            //    while (true)
            //    {
            //        if (result.Dictionary.ContainsKey("error_msg") || result.Dictionary.ContainsKey("error"))
            //        {
            //            //若發生error 檢查是何種error 若無error再透過exception離開迴圈
            //            string errorMessage = "";
            //            string errorNumber = "";
            //            if (result.Dictionary.ContainsKey("error_msg"))
            //            {
            //                errorMessage = result.Dictionary["error_msg"].String;
            //                errorNumber = result.Dictionary["error_code"].String;
            //            }
            //            else
            //            {
            //                errorMessage = result.Dictionary["error"].Dictionary["message"].String;
            //                errorNumber = result.Dictionary["error"].Dictionary["code"].String;
            //            }
            //            //Console.WriteLine("Error message(" + errorNumber + ") is : " + errorMessage);
            //            if (errorNumber.Equals("602"))
            //            {
            //                MessageBox.Show(errorMessage);
            //                return;
            //            }
            //            else
            //            {
            //                Thread.Sleep(1000);
            //                erro_count++;
            //                if (erro_count > 30)
            //                {
            //                    MessageBox.Show(errorMessage);
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            //catch { }
            foreach (JSONObject pageData in result.Array)
            {
                sql = "insert ignore into pages_info (id,name,picture,link,category,website,founded,info,likes,talking_about_count,updatetime,feeds,comments,new_id) values (" +
                    "?id,?name,?picture,?link,?category,?website,?founded,?info,?likes,?talking_about_count,?updatetime,0,0,1)";
                //page_id,name,pic_small,page_url,website,founded,fan_count,general_info,type
                while (true)
                {
                    try
                    {
                        MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
                        thecmd.Parameters.AddWithValue("?id", pageData.Dictionary["page_id"].String);
                        thecmd.Parameters.AddWithValue("?name", pageData.Dictionary["name"].String);
                        thecmd.Parameters.AddWithValue("?picture", pageData.Dictionary["pic_small"].String);
                        thecmd.Parameters.AddWithValue("?link", pageData.Dictionary["page_url"].String);
                        thecmd.Parameters.AddWithValue("?category", pageData.Dictionary["type"].String);
                        thecmd.Parameters.AddWithValue("?website", pageData.Dictionary["website"].String);
                        thecmd.Parameters.AddWithValue("?founded", pageData.Dictionary["founded"].String);
                        thecmd.Parameters.AddWithValue("?info", pageData.Dictionary["general_info"].String);
                        thecmd.Parameters.AddWithValue("?likes", pageData.Dictionary["fan_count"].String);
                        thecmd.Parameters.AddWithValue("?talking_about_count", "0");
                        thecmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                        MySqlDataReader themyData = thecmd.ExecuteReader();
                        themyData.Close();
                        thecmd.Parameters.Clear();
                        break;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.StackTrace);
                        conn.closeMySqlConnection();
                        conn.MySqlConnect();
                    }
                }
            }

            conn.closeMySqlConnection();
            updatePageIDList();
            checkCanUpdate();
            MessageBox.Show(result.Array.Length.ToString() + "筆資料新增");
        }
        public void updatePageIDList()
        {
            pagesInfo.Items.Clear();
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "select id,name,new_id,timespent from pages_info where new_id >= 0";
            MySqlDataReader res = conn.mysql_query(sql);
            if (res != null)
            {
                while (res.Read())
                {
                    ListViewItem item = new ListViewItem(res.GetString(0));

                    item.SubItems.Add(res.GetString(1));
                    if (res.GetInt32(2) == 0)
                    {
                        item.SubItems.Add("否");
                    }
                    else
                    {
                        item.SubItems.Add("是");
                    }
                    if (res.GetString(3).Length > 0)
                    {
                        item.SubItems.Add(res.GetString(3));
                    }
                    else
                    {
                        item.SubItems.Add("Null");
                    }
                    pagesInfo.Items.Add(item);
                }
                res.Close();
            }
            conn.closeMySqlConnection();
        }
        /// <summary>
        /// return with id,name,picture,link,category,website,founded,likes,talking_about_count
        /// </summary>

        #region get pages information
        public List<string> pages_info(string url)
        {
            FBGraph api = new FBGraph();
            JSONObject fbJSON = api.FBGraphGet(url);
            while (true)
            {
                try
                {
                    if (fbJSON.Dictionary.ContainsKey("error_msg") || fbJSON.Dictionary.ContainsKey("error"))
                    {
                        string errorMessage = "";
                        string errorNumber = "";
                        if (fbJSON.Dictionary.ContainsKey("error_msg"))
                        {
                            errorMessage = fbJSON.Dictionary["error_msg"].String;
                            errorNumber = fbJSON.Dictionary["error_code"].String;
                        }
                        else
                        {
                            errorMessage = fbJSON.Dictionary["error"].Dictionary["message"].String;
                            errorNumber = fbJSON.Dictionary["error"].Dictionary["code"].String;
                        }

                        if (errorNumber.Equals("4"))
                        {
                            //(#4)為超出limit sleep 10秒後再繼續
                            Thread.Sleep(10 * 1000);
                            fbJSON = api.FBGraphGet(url);
                        }
                        else
                        {
                            messageLog(errorMessage);
                            //showMessage.AppendText(errorMessage + "\n");
                            return null;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch { break; }
            }
            List<string> allData = new List<string>();
            string id = "0";
            string name = " ";
            string picture = " ";
            string link = " ";
            string category = " ";
            string website = " ";
            string founded = " ";
            string info = " ";
            string likes = "0";
            string talking_about_count = "0";
            try
            {
                id = fbJSON.Dictionary["id"].String;
            }
            catch { }
            try
            {
                name = fbJSON.Dictionary["name"].String;
            }
            catch { }
            try
            {
                picture = fbJSON.Dictionary["picture"].String;
            }
            catch { }
            try
            {
                link = fbJSON.Dictionary["link"].String;
            }
            catch { }
            try
            {
                category = fbJSON.Dictionary["category"].String;
            }
            catch { }
            try
            {
                website = fbJSON.Dictionary["website"].String;
            }
            catch { }
            try
            {
                founded = fbJSON.Dictionary["founded"].String;
            }
            catch { }
            try
            {
                info = fbJSON.Dictionary["general_info"].String;
            }
            catch { }
            try
            {
                likes = fbJSON.Dictionary["likes"].String;
            }
            catch { }
            try
            {
                talking_about_count = fbJSON.Dictionary["talking_about_count"].String;
            }
            catch { }
            allData.Add(id);
            allData.Add(name);
            allData.Add(picture);
            allData.Add(link);
            allData.Add(category);
            allData.Add(website);
            allData.Add(founded);
            allData.Add(info);
            allData.Add(likes);
            allData.Add(talking_about_count);
            if (id.Equals("0"))
                Console.WriteLine("the url is " + url + "\n" + fbJSON.String);
            return allData;
        }
        #endregion
        private void updatePageLogsCheck_CheckedChanged(object sender, EventArgs e)
        {
            pagesLogUpdateF.Enabled = updatePageLogsCheck.Checked;
            //recentlyDays.Enabled = updatePageLogsCheck.Checked;
        }

        private void messageLog(string message)
        {
            if (showErrorMessageCheck.Checked)
            {
                showMessage.AppendText("@" + DateTime.Now.ToString() + ":" + message + "\n");
            }
        }

        private void startUpdate_Click(object sender, EventArgs e)
        {
            int updateCount = 0;
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                if (lvi.Checked)
                    updateCount++;
            }
            if (updateCount == 0)
            {
                MessageBox.Show("未選取欲更新的粉絲頁");
                return;
            }
            string newIDSince = "1-1-1";
            int updateFequence = 0;
            int recent = 7;
            if (newPageIDSinceCheck.Checked)
            {
                newIDSince = updateSinceDate.Value.Year.ToString() + "-" + updateSinceDate.Value.Month.ToString() + "-" + updateSinceDate.Value.Day.ToString();
            }
            if (updatePageLogsCheck.Checked)
            {
                updateFequence = Int32.Parse(pagesLogUpdateF.Text);
            }
            //startUpdateData(newIDSince);
            updatePageThread = new Thread(delegate() { startUpdateData(newIDSince); });
            updatePageThread.Start();
            //showMessage.AppendText("Since=" + newIDSince + "\n");
            //updateThread goUpdateList = new updateThread(showMessage, dbInfo, newIDSince, updateFequence, recent, startUpdate, pagesInfo, cancleUpdate);
            ////goUpdateList.run();
            //ThreadStart doRun = new ThreadStart(goUpdateList.run);
            //updatePageThread = new Thread(doRun);
            //updatePageThread.Start();
            startUpdate.Enabled = false;
            cancleUpdate.Enabled = true;
        }

        private void startUpdateData(string newIDSince)
        {
            bool continueUpdate = false;
            if (updatePageLogsCheck.Checked)
                continueUpdate = true;
            while (true)
            {
                DBConnection conn = new DBConnection(dbInfo);
                conn.MySqlConnect();
                string sql = "select * from access_token where 1";
                MySqlDataReader res = null;
                while (true)
                {
                    try
                    {
                        res = conn.mysql_query(sql);
                        res.Read();
                        break;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.StackTrace);
                        conn.closeMySqlConnection();
                        conn.MySqlConnect();
                    }
                }
                DateTime nextUpdateTime = DateTime.Now.AddDays((1.0) * (int)pagesLogUpdateF.Value);
                DateTime untileDate = DateTime.Now.AddDays((-1.0) * (int)recentlyDays.Value);
                string since = untileDate.Year.ToString() + "-" + untileDate.Month.ToString() + "-" + untileDate.Day.ToString();
                string access_token = res.GetString(0);
                res.Close();
                string IDString = "";
                foreach (ListViewItem lvi in pagesInfo.CheckedItems)
                {
                    IDString += lvi.Text + ",";
                }
                IDString = IDString.Substring(0, IDString.Length - 1);
                sql = "select id,name,new_id from pages_info where new_id >= 0 and id in (" + IDString + ")";
                List<string[]> pageList = new List<string[]>();
                while (true)
                {
                    try
                    {
                        pageList.Clear();
                        res = conn.mysql_query(sql);
                        break;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.StackTrace);
                        conn.closeMySqlConnection();
                        conn.MySqlConnect();
                    }
                }
                while (res.Read())
                {
                    string[] tmpPage = { res.GetString(0), res.GetString(1), res.GetString(2) };
                    pageList.Add(tmpPage);
                }
                res.Close();
                foreach (string[] pagesInfoArray in pageList)
                {
                    DateTime startingTime = DateTime.Now;
                    showMessage.AppendText("Now updating " + pagesInfoArray[1] + " at " + DateTime.Now.ToString() + "\n");
                    string url = "https://graph.facebook.com/" + pagesInfoArray[0];

                    //showMessage.AppendText("In firstThread since=" + newIDSince + "\n");
                    if (pagesInfoArray[2] == "1")
                    {
                        get_post(pagesInfoArray[0], access_token, newIDSince, null);
                        sql = "update pages_info set new_id = 0 where id = \"" + pagesInfoArray[0] + "\"";
                        while (true)
                        {
                            try
                            {
                                conn.mysql_query(sql).Close();
                                break;
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.StackTrace);
                                conn.closeMySqlConnection();
                                conn.MySqlConnect();
                            }
                        }
                    }
                    else
                    {
                        get_post(pagesInfoArray[0], access_token, since, null);
                    }
                    sql = "select count(*) from pages_posts where page_id = \"" + pagesInfoArray[0] + "\"";
                    while (true)
                    {
                        try
                        {
                            res = conn.mysql_query(sql);
                            res.Read();
                            break;
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.StackTrace);
                            conn.closeMySqlConnection();
                            conn.MySqlConnect();
                        }
                    }
                    string feedsCount = res.GetString(0);
                    res.Close();
                    sql = "select count(*) from pages_posts_comments where page_id = \"" + pagesInfoArray[0] + "\"";
                    while (true)
                    {
                        try
                        {
                            res = conn.mysql_query(sql);
                            res.Read();
                            break;
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.StackTrace);
                            conn.closeMySqlConnection();
                            conn.MySqlConnect();
                        }
                    }
                    string commentsCount = res.GetString(0);
                    res.Close();
                    List<string> pageInformation = pages_info(url);
                    if (pageInformation != null)
                    {
                        sql = "insert into pages_logs (id,name,picture,link,category,website,founded,info,likes,talking_about_count,updatetime,feeds,comments) values (" +
                            "?id,?name,?picture,?link,?category,?website,?founded,?info,?likes,?talking_about_count,?updatetime," + feedsCount + "," + commentsCount + ")";
                        while (true)
                        {
                            try
                            {
                                MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
                                thecmd.Parameters.AddWithValue("?id", pageInformation[0]);
                                thecmd.Parameters.AddWithValue("?name", pageInformation[1]);
                                thecmd.Parameters.AddWithValue("?picture", pageInformation[2]);
                                thecmd.Parameters.AddWithValue("?link", pageInformation[3]);
                                thecmd.Parameters.AddWithValue("?category", pageInformation[4]);
                                thecmd.Parameters.AddWithValue("?website", pageInformation[5]);
                                thecmd.Parameters.AddWithValue("?founded", pageInformation[6]);
                                thecmd.Parameters.AddWithValue("?info", pageInformation[7]);
                                thecmd.Parameters.AddWithValue("?likes", pageInformation[8]);
                                thecmd.Parameters.AddWithValue("?talking_about_count", pageInformation[9]);
                                thecmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                                MySqlDataReader themyData = thecmd.ExecuteReader();
                                themyData.Close();
                                thecmd.Parameters.Clear();
                                break;
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.StackTrace);
                                conn.closeMySqlConnection();
                                conn.MySqlConnect();
                            }
                        }
                    }
                    TimeSpan timeSpent = DateTime.Now.Subtract(startingTime);
                    sql = "update pages_info set timespent = \"" + timeSpent.TotalMinutes.ToString("#.#0") + "分鐘" + "\" where id = \"" + pagesInfoArray[0] + "\"";
                    while (true)
                    {
                        try
                        {
                            conn.mysql_query(sql).Close();
                            break;
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.StackTrace);
                            conn.closeMySqlConnection();
                            conn.MySqlConnect();
                        }
                    }
                    conn.closeMySqlConnection();
                    showMessage.AppendText(pagesInfoArray[1] + " was updated complete at " + DateTime.Now.ToString() + "\n");
                }
                if (continueUpdate)
                {
                    try
                    {
                        TimeSpan ts = nextUpdateTime.Subtract(DateTime.Now);
                        //七天更新一次
                        showMessage.AppendText("預計下次更新時間為" + nextUpdateTime.ToString() + "\n\n");
                        Thread.Sleep(Convert.ToInt32(ts.TotalMilliseconds));
                    }
                    catch { }
                }
                else
                {
                    showMessage.AppendText("更新結束 @" + DateTime.Now.ToString() + "\n");
                    startUpdate.Enabled = true;
                    cancleUpdate.Enabled = false;
                    break;
                }
            }
        }

        /// <summary>
        /// return with id,name,picture,link,category,website,founded,likes,talking_about_count
        /// </summary>

        #region insert post
        public void get_post(string pageID, string access_token, string since, string until)
        {
            //until = "2013-11-22";
            int limit = maxLimit;
            string url = "https://graph.facebook.com/" + pageID + "/feed?access_token=" + access_token + "&limit=" + limit.ToString();

            if (since != null)
            {
                url += "&since=" + since;
            }
            if (until != null)
            {
                url += "&until=" + until;
            }
            //showMessage.AppendText(url + "\n");
            FBGraph api = new FBGraph();
            JSONObject fbJSON = api.FBGraphGet(url);

            do
            {
                fbJSON = resultCheck(fbJSON, url, limit);
                //while (true)
                //{
                //    if (fbJSON.Dictionary == null || fbJSON.Dictionary.ContainsKey("error_msg") || fbJSON.Dictionary.ContainsKey("error"))
                //    {
                //        string errorMessage = "";
                //        string errorNumber = "";
                //        if (fbJSON.Dictionary == null)
                //        {
                //            errorMessage = "An unknown error occurred";
                //            errorNumber = "1";
                //        }
                //        else if (fbJSON.Dictionary.ContainsKey("error_msg"))
                //        {
                //            errorMessage = fbJSON.Dictionary["error_msg"].String;
                //            errorNumber = fbJSON.Dictionary["error_code"].String;
                //        }
                //        else
                //        {
                //            errorMessage = fbJSON.Dictionary["error"].Dictionary["message"].String;
                //            errorNumber = fbJSON.Dictionary["error"].Dictionary["code"].String;
                //        }
                //        messageLog(errorMessage);
                //        //showMessage.AppendText("@" + DateTime.Now.ToString() + ":" + errorMessage + "\n");
                //        if (errorNumber.Equals("4"))
                //        {
                //            //(#4)為超出limit sleep 10秒後再繼續
                //            Thread.Sleep(10 * 1000);
                //            fbJSON = api.FBGraphGet(url);
                //        }
                //        else if (errorNumber.Equals("1"))
                //        {
                //            if (limit <= 25)
                //            {
                //                return;
                //            }
                //            limit = limit / 2;
                //            if (limit <= 25)
                //                limit = 25;
                //            url = "https://graph.facebook.com/" + pageID + "/feed?access_token=" + access_token + "&limit=" + limit.ToString();

                //            if (since != null)
                //            {
                //                url += "&since=" + since;
                //            }
                //            if (until != null)
                //            {
                //                url += "&until=" + until;
                //            }
                //            Thread.Sleep(10 * 1000);
                //            fbJSON = api.FBGraphGet(url);
                //        }
                //        else
                //        {
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    //try
                //    //{
                //    //    string errorMessage = fbJSON.Dictionary["error_msg"].String;
                //    //    string errorNumber = fbJSON.Dictionary["error_code"].String;
                //    //    messageLog(errorMessage);
                //    //    //showMessage.AppendText("@" + DateTime.Now.ToString() + ":" + errorMessage + "\n");
                //    //    if (errorNumber.Equals("4"))
                //    //    {
                //    //        //(#4)為超出limit sleep 10秒後再繼續
                //    //        Thread.Sleep(10 * 1000);
                //    //        fbJSON = api.FBGraphGet(url);
                //    //    }
                //    //    else if (errorNumber.Equals("1"))
                //    //    {
                //    //        if (limit <= 25)
                //    //        {
                //    //            return;
                //    //        }
                //    //        limit = limit / 2;
                //    //        if (limit <= 25)
                //    //            limit = 25;
                //    //        url = "https://graph.facebook.com/" + pageID + "/feed?access_token=" + access_token + "&limit=" + limit.ToString();

                //    //        if (since != null)
                //    //        {
                //    //            url += "&since=" + since;
                //    //        }
                //    //        if (until != null)
                //    //        {
                //    //            url += "&until=" + until;
                //    //        }
                //    //        Thread.Sleep(10 * 1000);
                //    //        fbJSON = api.FBGraphGet(url);
                //    //    }
                //    //    else
                //    //    {
                //    //        return;
                //    //    }
                //    //}
                //    //catch { break; }
                //}
                DBConnection conn = new DBConnection(dbInfo);
                conn.MySqlConnect();
                try
                {
                    foreach (JSONObject post in fbJSON.Dictionary["data"].Array)
                    {
                        string postID = " ";
                        string fromID = " ";
                        string fromName = " ";
                        string message = " ";
                        string picture = " ";
                        string link = " ";
                        string name = " ";
                        string caption = " ";
                        string source = " ";
                        string icon = " ";
                        string type = " ";
                        string objectID = " ";
                        string description = " ";
                        string likes = "0";
                        string comments = "0";
                        string shares = "0";
                        DateTime postCreatedTime = DateTime.Now;
                        DateTime postUpdateTime = DateTime.Now;

                        if (post.Dictionary.ContainsKey("id"))
                        {
                            postID = post.Dictionary["id"].String;
                        }
                        if (post.Dictionary.ContainsKey("created_time"))
                        {
                            postCreatedTime = fbDatetimeToDatetime(post.Dictionary["created_time"].String);
                        }
                        if (post.Dictionary.ContainsKey("updated_time"))
                        {
                            postUpdateTime = fbDatetimeToDatetime(post.Dictionary["updated_time"].String);
                        }
                        if (post.Dictionary.ContainsKey("from"))
                        {
                            if (post.Dictionary["from"].Dictionary.ContainsKey("id"))
                                fromID = post.Dictionary["from"].Dictionary["id"].String;
                            if (post.Dictionary["from"].Dictionary.ContainsKey("name"))
                                fromName = post.Dictionary["from"].Dictionary["name"].String;
                        }
                        if (post.Dictionary.ContainsKey("message"))
                        {
                            message = post.Dictionary["message"].String;
                        }
                        if (post.Dictionary.ContainsKey("picture"))
                        {
                            picture = post.Dictionary["picture"].String;
                        }
                        if (post.Dictionary.ContainsKey("link"))
                        {
                            link = post.Dictionary["link"].String;
                        }
                        if (post.Dictionary.ContainsKey("name"))
                        {
                            name = post.Dictionary["name"].String;
                        }
                        if (post.Dictionary.ContainsKey("caption"))
                        {
                            caption = post.Dictionary["caption"].String;
                        }
                        if (post.Dictionary.ContainsKey("source"))
                        {
                            source = post.Dictionary["source"].String;
                        }
                        if (post.Dictionary.ContainsKey("icon"))
                        {
                            icon = post.Dictionary["icon"].String;
                        }
                        if (post.Dictionary.ContainsKey("type"))
                        {
                            type = post.Dictionary["type"].String;
                        }
                        if (post.Dictionary.ContainsKey("object_id"))
                        {
                            objectID = post.Dictionary["object_id"].String;
                        }
                        if (post.Dictionary.ContainsKey("description"))
                        {
                            description = post.Dictionary["description"].String;
                        }
                        if (post.Dictionary.ContainsKey("likes"))
                        {
                            likes = get_likes(pageID, postID, access_token, post.Dictionary["likes"], postCreatedTime, "").ToString();
                        }
                        if (post.Dictionary.ContainsKey("comments"))
                        {
                            comments = get_post_comment(pageID, postID, access_token, post.Dictionary["comments"], postCreatedTime, "").ToString();
                        }
                        if (post.Dictionary.ContainsKey("share"))
                        {
                            shares = post.Dictionary["shares"].Dictionary["count"].String;
                        }
                        string sql = "insert into pages_posts (post_id,page_id,from_id,from_name,message,picture,link,name,caption,source,icon,type,object_id,description,likes,created_time,updated_time,comments,shares,updatetime) values (" +
                            "\"" + postID + "\",\"" + pageID + "\",\"" + fromID + "\",?fname,?message,?picture,?link,?name,?caption,?source,?icon,?type,?object_id,?description," + likes + ",?created_time,?update_time," + comments + "," + shares + ",?updatetime) " +
                            "ON DUPLICATE KEY UPDATE message =?message, likes=" + likes + ",comments=" + comments + ",shares=" + shares + ",updated_time=?update_time,updatetime=?updatetime";
                        //showMessage.AppendText(sql + "\n");
                        for (int loop = 0; loop < 2; loop++)
                        {
                            try
                            {
                                MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
                                thecmd.Parameters.AddWithValue("?fname", fromName);
                                thecmd.Parameters.AddWithValue("?message", message);
                                thecmd.Parameters.AddWithValue("?picture", picture);
                                thecmd.Parameters.AddWithValue("?link", link);
                                thecmd.Parameters.AddWithValue("?name", name);
                                thecmd.Parameters.AddWithValue("?caption", caption);
                                thecmd.Parameters.AddWithValue("?source", source);
                                thecmd.Parameters.AddWithValue("?icon", icon);
                                thecmd.Parameters.AddWithValue("?type", type);
                                thecmd.Parameters.AddWithValue("?object_id", objectID);
                                thecmd.Parameters.AddWithValue("?description", description);
                                thecmd.Parameters.AddWithValue("?created_time", postCreatedTime);
                                thecmd.Parameters.AddWithValue("?update_time", postUpdateTime);
                                thecmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                                MySqlDataReader themyData = thecmd.ExecuteReader();
                                themyData.Close();
                                thecmd.Parameters.Clear();
                                break;
                                //showMessage.AppendText("Insert complete!!\n");
                            }
                            catch (Exception sqlE)
                            {
                                Console.WriteLine(sqlE.StackTrace);
                                conn.closeMySqlConnection();
                                conn.MySqlConnect();
                            }
                        }
                    }
                }
                catch
                {
                    break;
                }
                conn.closeMySqlConnection();
                //get next page data
                try
                {
                    url = fbJSON.Dictionary["paging"].Dictionary["next"].String;
                }
                catch
                {
                    return;
                }
                if (since != null)
                    url += "&since=" + since;
                url += "&limit=" + limit.ToString();
                fbJSON = api.FBGraphGet(url);
                fbJSON = resultCheck(fbJSON, url, limit);
                //while (true)
                //{
                //    if (fbJSON.Dictionary == null || fbJSON.Dictionary.ContainsKey("error") || fbJSON.Dictionary.ContainsKey("error_msg"))
                //    {
                //        string errorMessage = "";
                //        string errorNumber = "";
                //        if (fbJSON.Dictionary == null)
                //        {
                //            errorMessage = "An unknown error occurred";
                //            errorNumber = "1";
                //        }
                //        else if (fbJSON.Dictionary.ContainsKey("error_msg"))
                //        {
                //            errorMessage = fbJSON.Dictionary["error_msg"].String;
                //            errorNumber = fbJSON.Dictionary["error_code"].String;
                //        }
                //        else
                //        {
                //            errorMessage = fbJSON.Dictionary["error"].Dictionary["message"].String;
                //            errorNumber = fbJSON.Dictionary["error"].Dictionary["code"].String;
                //        }
                //        if (errorNumber.Equals("4"))
                //        {
                //            //(#4)為超出limit sleep 10秒後再繼續
                //            Thread.Sleep(10 * 1000);
                //            fbJSON = api.FBGraphGet(url);
                //        }
                //        else { break; }
                //    }
                //    else { break; }
                //}
                try
                {
                    if (fbJSON.Dictionary["data"].Array.Length > 0)
                    {
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
            } while (fbJSON.Dictionary["data"].Array.Length > 0);
        }
        #endregion

        private DateTime fbDatetimeToDatetime(string fbdate)
        {
            //EX: 2013-07-28T19:18:24+0000
            string[] splitter = { "-", "T", ":", "+" };
            try
            {
                string[] timePart = fbdate.Split(splitter, StringSplitOptions.None);
                DateTime date = new DateTime(Convert.ToInt32(timePart[0]), Convert.ToInt32(timePart[1]), Convert.ToInt32(timePart[2]), Convert.ToInt32(timePart[3]), Convert.ToInt32(timePart[4]), Convert.ToInt32(timePart[5]));
                return date.AddHours(8);//Convert to TW time
            }
            catch (Exception te)
            {
                Console.WriteLine("Convert Time fromat exception with \"" + fbdate + "\"" + te.ToString());
                return new DateTime(1979, 1, 1);
            }
        }

        #region insert post comment
        public int get_post_comment(string pageID, string postID, string access_token, JSONObject commJSON, DateTime postCreatedTime, string nextUrl)
        {
            int totoalCount = 0;
            string url = "https://graph.facebook.com/" + postID + "/comments?access_token=" + access_token + "&limit=" + commentLimit.ToString() + "&summary=1";
            if (nextUrl.Length > 0)
                url = nextUrl + "&limit=" + commentLimit.ToString();
            commJSON = new FBGraph().FBGraphGet(url);
            commJSON = resultCheck(commJSON, url, commentLimit);
            if (commJSON == null)
                return totoalCount;
            //while (true)
            //{
            //    if (commJSON.Dictionary == null || commJSON.Dictionary.ContainsKey("error") || commJSON.Dictionary.ContainsKey("error_msg"))
            //    {
            //        string errorMessage = "";
            //        string errorNumber = "";
            //        if (commJSON.Dictionary == null)
            //        {
            //            errorMessage = "An unknown error occurred";
            //            errorNumber = "1";
            //        }
            //        else if (commJSON.Dictionary.ContainsKey("error_msg"))
            //        {
            //            errorMessage = commJSON.Dictionary["error_msg"].String;
            //            errorNumber = commJSON.Dictionary["error_code"].String;
            //        }
            //        else
            //        {
            //            errorMessage = commJSON.Dictionary["error"].Dictionary["message"].String;
            //            errorNumber = commJSON.Dictionary["error"].Dictionary["code"].String;
            //        }
            //        messageLog(errorMessage);
            //        //showMessage.AppendText("@" + DateTime.Now.ToString() + ":" + errorMessage + "\n");
            //        if (errorNumber.Equals("4") || errorNumber.Equals("2"))
            //        {
            //            //(#4)為超出limit sleep 30秒後再繼續
            //            Thread.Sleep(30 * 1000);
            //            commJSON = new FBGraph().FBGraphGet(url);
            //        }
            //        else
            //        {
            //            return totoalCount;
            //        }
            //    }
            //    else { break; }
            //}
            Dictionary<string, string> fansName = new Dictionary<string, string>();
            //bool hasNaxt = false;
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "";
            try
            {
                totoalCount = Convert.ToInt32(commJSON.Dictionary["summary"].Dictionary["total_count"].String);
            }
            catch { }
            if (commJSON.Dictionary["data"].Array.Length > 0)
            {
                foreach (JSONObject comment in commJSON.Dictionary["data"].Array)
                {
                    string fromID = "";
                    string fromName = "";
                    string commID = "";
                    string message = "";
                    string likes = "0";
                    DateTime created_time = DateTime.Now;
                    if (comment.Dictionary.ContainsKey("id"))
                    {
                        commID = comment.Dictionary["id"].String;
                    }
                    else
                    {
                        Console.WriteLine("In Comment fetch exception with comment id\n");
                        continue;
                    }
                    if (comment.Dictionary.ContainsKey("from"))
                    {
                        if (comment.Dictionary["from"].Dictionary.ContainsKey("id"))
                            fromID = comment.Dictionary["from"].Dictionary["id"].String;
                        if (comment.Dictionary["from"].Dictionary.ContainsKey("name"))
                            fromName = comment.Dictionary["from"].Dictionary["name"].String;
                        if (!fansName.ContainsKey(fromID))
                        {
                            fansName.Add(fromID, fromName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("In Comment fetch exception with id & name\n");
                        continue;
                    }
                    if (comment.Dictionary.ContainsKey("message"))
                    {
                        message = comment.Dictionary["message"].String;
                    }
                    else
                    {
                        Console.WriteLine("In Comment fetch exception with comment message\n");
                    }
                    if (comment.Dictionary.ContainsKey("like_count"))
                    {
                        likes = comment.Dictionary["like_count"].String;
                    }
                    if (comment.Dictionary.ContainsKey("created_time"))
                    {
                        created_time = fbDatetimeToDatetime(comment.Dictionary["created_time"].String);
                    }
                    else
                    {
                        Console.WriteLine("In Comment fetch exception with comment created_time \n");
                    }
                    sql = "insert ignore into pages_posts_comments (comment_id,page_id,post_id,from_id,from_name,message,created_time,post_created_time,likes,updatetime) values (" +
                        "?comment_id,?page_id,?post_id,?from_id,?from_name,?message,?created_time,?post_created_time,?likes,?updatetime) " +
                        "ON DUPLICATE KEY UPDATE likes=?likes,updatetime=?updatetime";
                    while (true)
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
                            cmd.Parameters.AddWithValue("?comment_id", commID);
                            cmd.Parameters.AddWithValue("?page_id", pageID);
                            cmd.Parameters.AddWithValue("?post_id", postID);
                            cmd.Parameters.AddWithValue("?from_id", fromID);
                            cmd.Parameters.AddWithValue("?from_name", fromName);
                            cmd.Parameters.AddWithValue("?message", message);
                            cmd.Parameters.AddWithValue("?created_time", created_time);
                            cmd.Parameters.AddWithValue("?post_created_time", postCreatedTime);
                            cmd.Parameters.AddWithValue("?likes", likes);
                            cmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                            cmd.ExecuteReader().Close();
                            //totoalCount++;
                            break;
                        }
                        catch (Exception ce)
                        {
                            Console.WriteLine(ce.StackTrace);
                            conn.closeMySqlConnection();
                            conn.MySqlConnect();
                        }
                    }
                }
                if (fansName.Count > 0)
                {
                    insert_pages_fans(fansName, pageID);
                }
                //string after = "";
                conn.closeMySqlConnection();
                if (commJSON.Dictionary.ContainsKey("paging"))
                {
                    if (commJSON.Dictionary["paging"].Dictionary.ContainsKey("next"))
                    {
                        url = commJSON.Dictionary["paging"].Dictionary["next"].String;
                        get_post_comment(pageID, postID, access_token, null, postCreatedTime, url);
                    }
                    //hasNaxt = true;
                }
            }
            else
            {
                conn.closeMySqlConnection();
            }
            return totoalCount;
        }
        #endregion

        private void insert_posts_likes(Dictionary<string, string> fansName, string pageID, string postID, DateTime createdTime)
        {
            string sql = "";
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert ignore into pages_posts_likes (post_id,page_id,id,name,post_created_time,updatetime) values ");
            foreach (KeyValuePair<string, string> kvp in fansName)
            {
                sb.Append("(?postID," + pageID + "," + kvp.Key + ",?" + kvp.Key + ",?createdTime,?updatetime),");
            }
            sql = sb.ToString();
            sql = sql.Substring(0, sql.Length - 1);
            while (true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
                    cmd.Parameters.AddWithValue("?postID", postID);
                    cmd.Parameters.AddWithValue("?createdTime", createdTime);
                    cmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                    foreach (KeyValuePair<string, string> kvp in fansName)
                    {
                        cmd.Parameters.AddWithValue("?" + kvp.Key, kvp.Value);
                    }
                    cmd.ExecuteReader().Close();
                    break;
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.StackTrace);
                    conn.closeMySqlConnection();
                    conn.MySqlConnect();
                }
            }

        }

        private void insert_pages_fans(Dictionary<string, string> fansName, string pageID)
        {
            string sql = "";
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            StringBuilder sb = new StringBuilder();
            StringBuilder fansInfo = new StringBuilder();
            sb.Append("insert ignore into pages_fans (pages_id,id,name) values ");
            fansInfo.Append("insert ignore into user_info (id,name,username,gender,updatetime) values ");
            foreach (KeyValuePair<string, string> kvp in fansName)
            {
                sb.Append("(" + pageID + "," + kvp.Key + ",?" + kvp.Key + "),");
                fansInfo.Append("(" + kvp.Key + ",?" + kvp.Key + ",'','',?updatetime),");
            }
            sql = sb.ToString();
            sql = sql.Substring(0, sql.Length - 1);
            while (true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
                    foreach (KeyValuePair<string, string> kvp in fansName)
                    {
                        cmd.Parameters.AddWithValue("?" + kvp.Key, kvp.Value);
                    }
                    cmd.ExecuteReader().Close();
                    break;
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.StackTrace);
                    conn.closeMySqlConnection();
                    conn.MySqlConnect();
                }
            }
            sql = fansInfo.ToString();
            sql = sql.Substring(0, sql.Length - 1);
            while (true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
                    cmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                    foreach (KeyValuePair<string, string> kvp in fansName)
                    {
                        cmd.Parameters.AddWithValue("?" + kvp.Key, kvp.Value);
                    }
                    cmd.ExecuteReader().Close();
                    break;
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.StackTrace);
                    conn.closeMySqlConnection();
                    conn.MySqlConnect();
                }
            }
        }
        #region insert likes
        public int get_likes(string pageID, string objectID, string access_token, JSONObject likeJSON, DateTime postCreatedTime, string nextUrl)
        {
            int totoalCount = 0;
            //string url = "https://graph.facebook.com/" + postID + "/comments?access_token=" + access_token + "&limit=500";
            string url = "https://graph.facebook.com/" + objectID + "/likes?access_token=" + access_token + "&limit=" + likeLimit.ToString() + "&summary=1";
            if (nextUrl.Length > 0)
                url = nextUrl + "&limit=" + likeLimit.ToString();
            likeJSON = new FBGraph().FBGraphGet(url);
            likeJSON = resultCheck(likeJSON, url, likeLimit);
            if (likeJSON == null)
                return totoalCount;
            //while (true)
            //{
            //    if (likeJSON.Dictionary == null || likeJSON.Dictionary.ContainsKey("error") || likeJSON.Dictionary.ContainsKey("error_msg"))
            //    {
            //        string errorMessage = "";
            //        string errorNumber = "";
            //        if (likeJSON.Dictionary == null)
            //        {
            //            errorMessage = "An unknown error occurred";
            //            errorNumber = "1";
            //        }
            //        else if (likeJSON.Dictionary.ContainsKey("error_msg"))
            //        {
            //            errorMessage = likeJSON.Dictionary["error_msg"].String;
            //            errorNumber = likeJSON.Dictionary["error_code"].String;
            //        }
            //        else
            //        {
            //            errorMessage = likeJSON.Dictionary["error"].Dictionary["message"].String;
            //            errorNumber = likeJSON.Dictionary["error"].Dictionary["code"].String;
            //        }
            //        messageLog(errorMessage);
            //        //showMessage.AppendText("@" + DateTime.Now.ToString() + ":" + errorMessage + "\n");
            //        if (errorNumber.Equals("4") || errorNumber.Equals("2"))
            //        {
            //            //(#4)為超出limit sleep 30秒後再繼續
            //            Thread.Sleep(30 * 1000);
            //            likeJSON = new FBGraph().FBGraphGet(url);
            //        }
            //        else
            //        {
            //            return totoalCount;
            //        }
            //    }
            //    else { break; }
            //}
            Dictionary<string, string> fansName = new Dictionary<string, string>();
            //bool hasNaxt = false;
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "";
            try
            {
                totoalCount = Convert.ToInt32(likeJSON.Dictionary["summary"].Dictionary["total_count"].String);
            }
            catch { }
            if (likeJSON.Dictionary["data"].Array.Length > 0)
            {
                foreach (JSONObject ljson in likeJSON.Dictionary["data"].Array)
                {
                    string fromID = "";
                    string fromName = "";

                    if (ljson.Dictionary.ContainsKey("id"))
                    {
                        fromID = ljson.Dictionary["id"].String;
                        if (ljson.Dictionary.ContainsKey("name"))
                            fromName = ljson.Dictionary["name"].String;
                        if (!fansName.ContainsKey(fromID))
                        {
                            fansName.Add(fromID, fromName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("In Comment fetch exception with id & name\n");
                        messageLog("In Comment fetch exception with id & name\n");
                        continue;
                    }
                }
                if (fansName.Count > 0)
                {
                    insert_pages_fans(fansName, pageID);
                    insert_posts_likes(fansName, pageID, objectID, postCreatedTime);
                }
                conn.closeMySqlConnection();
                //string after = "";
                if (likeJSON.Dictionary.ContainsKey("paging"))
                {
                    if (likeJSON.Dictionary["paging"].Dictionary.ContainsKey("next"))
                    {
                        url = likeJSON.Dictionary["paging"].Dictionary["next"].String;
                        get_likes(pageID, objectID, access_token, null, postCreatedTime, url);
                    }
                    //hasNaxt = true;
                }
            }
            return totoalCount;
        }
        #endregion
        private void deletePageID_Click(object sender, EventArgs e)
        {
            int deleteCount = 0;
            string deleteID = "";
            string deleteResultMessage = "";
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                if (lvi.Checked)
                {
                    deleteID += lvi.SubItems[0].Text + ",";
                    deleteResultMessage += lvi.SubItems[1].Text + "(" + lvi.SubItems[0].Text + ")\n";
                    deleteCount++;
                }
            }
            if (deleteCount == 0)
            {
                MessageBox.Show("請先勾選要刪除的粉絲頁");
                return;
            }
            deleteID = deleteID.Substring(0, deleteID.Length - 1);
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "delete from pages_info where id in (" + deleteID + ")";
            try
            {
                conn.mysql_query(sql).Close();
            }
            catch { }
            sql = "delete from pages_logs where id in (" + deleteID + ")";
            try
            {
                conn.mysql_query(sql).Close();
            }
            catch { }
            sql = "delete from pages_posts where page_id in (" + deleteID + ")";
            try
            {
                conn.mysql_query(sql).Close();
            }
            catch { }
            sql = "delete from pages_posts_comments where page_id in (" + deleteID + ")";
            try
            {
                conn.mysql_query(sql).Close();
            }
            catch { }
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                if (lvi.Checked)
                {
                    sql = "delete from pages_posts_comments_likes where comment_id like \"" + lvi.SubItems[0].Text + "_%\"";
                    conn.mysql_query(sql).Close();
                    sql = "delete from pages_posts_likes where post_id like \"" + lvi.SubItems[0].Text + "_%\"";
                    conn.mysql_query(sql).Close();
                }
            }
            conn.closeMySqlConnection();
            updatePageIDList();
            checkCanUpdate();
            MessageBox.Show(deleteResultMessage + "已被刪除");
        }

        private void reflash_Click(object sender, EventArgs e)
        {
            updatePageIDList();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (searchKeyword.Text.Length == 0)
            {
                MessageBox.Show("請先輸入搜尋關鍵詞");
                return;
            }
            searchButton.Text = "搜尋中…";
            searchButton.Enabled = false;
            searchResultList.Items.Clear();
            //string url = "https://graph.facebook.com/search?q=" + searchKeyword.Text + "&type=page&access_token=" + accessTokenBox.Text + "&limit=" + searchLimit.Value.ToString();
            string url = "https://graph.facebook.com/search?q=" + searchKeyword.Text + "&type=page&access_token=" + accessTokenBox.Text + "&limit=500";
            //showMessage.AppendText(url + "\n");
            FBGraph api = new FBGraph();
            JSONObject result = api.FBGraphGet(url);
            try
            {
                if (!result.Dictionary.ContainsKey("data") || result.Dictionary["data"].Array.Length == 0)
                {
                    MessageBox.Show("使用『" + searchKeyword.Text + "』查無資料！");

                    searchButton.Enabled = true;
                    searchButton.Text = "粉絲頁搜尋";
                    return;
                }
            }
            catch (Exception graphE)
            {
                MessageBox.Show("錯誤訊息：" + graphE.ToString());

                searchButton.Enabled = true;
                searchButton.Text = "粉絲頁搜尋";
                return;
            }
            Dictionary<string, string[]> resultPage = new Dictionary<string, string[]>();
            string IDstring = "";
            foreach (JSONObject pageData in result.Dictionary["data"].Array)
            {
                string[] data = new string[3];
                data[0] = pageData.Dictionary["name"].String;
                data[2] = pageData.Dictionary["category"].String;
                data[1] = "0";
                resultPage.Add(pageData.Dictionary["id"].String, data);
                IDstring += pageData.Dictionary["id"].String + ",";
            }
            IDstring = IDstring.Substring(0, IDstring.Length - 1);
            DateTime start = DateTime.Now;
            url = "https://api.facebook.com/method/fql.query?query=select page_id, fan_count from page where page_id in (" + IDstring + ")&access_token=" + accessTokenBox.Text + "&format=json";
            //showMessage.AppendText(url + "\n");
            result = api.FBGraphGet(url);
            //showMessage.AppendText(DateTime.Now.Subtract(start).TotalSeconds.ToString(".00") + " s\n");
            if (result.Dictionary != null && (result.Dictionary.ContainsKey("error_msg") || result.Dictionary.ContainsKey("error")))
            {
                int erro_count = 0;
                while (true)
                {
                    //若發生error 檢查是何種error 若無error再透過exception離開迴圈
                    string errorMessage = "";
                    string errorNumber = "";
                    if (result.Dictionary.ContainsKey("error_msg"))
                    {
                        errorMessage = result.Dictionary["error_msg"].String;
                        errorNumber = result.Dictionary["error_code"].String;
                    }
                    else
                    {
                        errorMessage = result.Dictionary["error"].Dictionary["message"].String;
                        errorNumber = result.Dictionary["error"].Dictionary["code"].String;
                    }
                    //Console.WriteLine("Error message(" + errorNumber + ") is : " + errorMessage);
                    if (errorNumber.Equals("602"))
                    {
                        MessageBox.Show(errorMessage);

                        searchButton.Enabled = true;
                        searchButton.Text = "粉絲頁搜尋";
                        return;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        erro_count++;
                        if (erro_count > 30)
                        {
                            MessageBox.Show(errorMessage);

                            searchButton.Enabled = true;
                            searchButton.Text = "粉絲頁搜尋";
                            return;
                        }
                    }

                }
            }
            foreach (JSONObject pageData in result.Array)
            {
                resultPage[pageData.Dictionary["page_id"].String][1] = pageData.Dictionary["fan_count"].String;
            }
            int listCount = 0;
            foreach (KeyValuePair<string, string[]> kvp in resultPage)
            {

                ListViewItem item = new ListViewItem(kvp.Key);
                for (int i = 0; i < kvp.Value.Length; i++)
                {
                    item.SubItems.Add(kvp.Value[i]);
                }
                if (kvp.Value[0].Contains(searchKeyword.Text))
                {
                    searchResultList.Items.Add(item);
                    listCount++;
                    if (listCount > (int)searchLimit.Value)
                        break;
                }
            }
            searchButton.Enabled = true;
            searchButton.Text = "粉絲頁搜尋";
        }

        private void cancleUpdate_Click(object sender, EventArgs e)
        {
            if (updatePageThread != null)
            {
                updatePageThread.Abort();
                startUpdate.Enabled = true;
                cancleUpdate.Enabled = false;
                showMessage.AppendText("已取消更新！\n");
            }
        }

        private void post_comment_search_Click(object sender, EventArgs e)
        {
            SearchForm sForm = new SearchForm(dbInfo);
            sForm.Show(this);
        }

        private void searchResultList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            resultBool[e.Column] ^= true;
            //showMessage.AppendText(resultBool[e.Column].ToString() + "\n");
            searchResultList.ListViewItemSorter = new MyListViewSorter(e.Column, resultBool[e.Column], e.Column.Equals(2));
            searchResultList.Sort();
        }

        private void clearResultCheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in searchResultList.Items)
            {
                lvi.Checked = false;
            }
        }

        private void clearPagesCheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                lvi.Checked = false;
            }
        }

        private void selectAllPageButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                lvi.Checked = true;
            }
        }

        private void unselectAllPageButton_Click(object sender, EventArgs e)
        {
            //update_user_info();
            foreach (ListViewItem lvi in pagesInfo.Items)
            {
                lvi.Checked = false;
            }
        }

        private void selectAllResultPageButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in searchResultList.Items)
            {
                lvi.Checked = true;
            }
        }

        private void unselectAllResultPageButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in searchResultList.Items)
            {
                lvi.Checked = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void userInfoTimer_Tick(object sender, EventArgs e)
        {
            Thread updateThread = new Thread(delegate() { update_user_info(); });
            updateThread.Start();
        }

        private void addWithManualPageID_Click(object sender, EventArgs e)
        {
            if (mainualPageIDs.Text.Length == 0)
                return;
            string sql = "";
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string IDString = mainualPageIDs.Text;
            string url = "https://api.facebook.com/method/fql.query?query=select page_id,name,pic_small,page_url,website,founded,fan_count,general_info,type from page where page_id in (" + IDString + ")&access_token=" + accessTokenBox.Text + "&format=json";
            FBGraph api = new FBGraph();
            JSONObject result = api.FBGraphGet(url);
            result = resultCheck(result, url, 0);
            if (result == null)
                return;
            //try
            //{
            //    int erro_count = 0;
            //    while (true)
            //    {
            //        if (result.Dictionary.ContainsKey("error_msg") || result.Dictionary.ContainsKey("error"))
            //        {
            //            //若發生error 檢查是何種error 若無error再透過exception離開迴圈
            //            string errorMessage = "";
            //            string errorNumber = "";
            //            if (result.Dictionary.ContainsKey("error_msg"))
            //            {
            //                errorMessage = result.Dictionary["error_msg"].String;
            //                errorNumber = result.Dictionary["error_code"].String;
            //            }
            //            else
            //            {
            //                errorMessage = result.Dictionary["error"].Dictionary["message"].String;
            //                errorNumber = result.Dictionary["error"].Dictionary["code"].String;
            //            }
            //            //Console.WriteLine("Error message(" + errorNumber + ") is : " + errorMessage);
            //            if (errorNumber.Equals("602"))
            //            {
            //                MessageBox.Show(errorMessage);
            //                return;
            //            }
            //            else
            //            {
            //                Thread.Sleep(1000);
            //                erro_count++;
            //                if (erro_count > 30)
            //                {
            //                    MessageBox.Show(errorMessage);
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            break;
            //        }

            //    }
            //}
            //catch { }
            foreach (JSONObject pageData in result.Array)
            {
                sql = "insert ignore into pages_info (id,name,picture,link,category,website,founded,info,likes,talking_about_count,updatetime,feeds,comments,new_id) values (" +
                    "?id,?name,?picture,?link,?category,?website,?founded,?info,?likes,?talking_about_count,?updatetime,0,0,1)";
                //page_id,name,pic_small,page_url,website,founded,fan_count,general_info,type
                while (true)
                {
                    try
                    {
                        MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
                        thecmd.Parameters.AddWithValue("?id", pageData.Dictionary["page_id"].String);
                        thecmd.Parameters.AddWithValue("?name", pageData.Dictionary["name"].String);
                        thecmd.Parameters.AddWithValue("?picture", pageData.Dictionary["pic_small"].String);
                        thecmd.Parameters.AddWithValue("?link", pageData.Dictionary["page_url"].String);
                        thecmd.Parameters.AddWithValue("?category", pageData.Dictionary["type"].String);
                        thecmd.Parameters.AddWithValue("?website", pageData.Dictionary["website"].String);
                        thecmd.Parameters.AddWithValue("?founded", pageData.Dictionary["founded"].String);
                        thecmd.Parameters.AddWithValue("?info", pageData.Dictionary["general_info"].String);
                        thecmd.Parameters.AddWithValue("?likes", pageData.Dictionary["fan_count"].String);
                        thecmd.Parameters.AddWithValue("?talking_about_count", "0");
                        thecmd.Parameters.AddWithValue("?updatetime", DateTime.Now);
                        MySqlDataReader themyData = thecmd.ExecuteReader();
                        themyData.Close();
                        thecmd.Parameters.Clear();
                        break;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.StackTrace);
                        conn.closeMySqlConnection();
                        conn.MySqlConnect();
                    }
                }
            }

            conn.closeMySqlConnection();
            updatePageIDList();
            checkCanUpdate();
            MessageBox.Show(result.Array.Length.ToString() + "筆資料新增");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (File.Exists(dbInfoFileName))
            {
                StreamReader sre = new StreamReader(dbInfoFileName);
                if (sre.Peek() > 0)
                {
                    string line = sre.ReadLine().Trim();
                    string[] dbData = line.Split(':');
                    if (dbData.Length == 4)
                    {
                        dbServerBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(dbData[0]));
                        dbAcoountBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(dbData[1]));
                        dbPasswordBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(dbData[2]));
                        dbTableBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(dbData[3]));
                    }
                }
                sre.Close();
            }
        }
    }
}

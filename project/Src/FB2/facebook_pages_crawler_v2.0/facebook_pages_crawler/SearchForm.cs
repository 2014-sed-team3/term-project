using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;

namespace facebook_pages_crawler
{
    public partial class SearchForm : Form
    {
        List<string> dbInfo;
        Dictionary<string, string[]> postMessage = new Dictionary<string, string[]>();
        Dictionary<string, string[]> commentMessage = new Dictionary<string, string[]>();
        Dictionary<string, string[]> allCommentMessage = new Dictionary<string, string[]>();
        Dictionary<string, string> pageName = new Dictionary<string, string>();
        string postString = "Post";
        string commentString = "Comment";
        bool[] postSort;
        bool commentSort = false;
        int IDIndex = 6;
        List<string> pagesID = new List<string>();
        List<string> pagesName = new List<string>();
        public SearchForm(List<string> dbInfo)
        {
            InitializeComponent();
            this.dbInfo = dbInfo;
            postSort = new bool[levelOneList.Columns.Count];
            string sql = "select id, name from pages_info";
            DBConnection conn = new DBConnection(this.dbInfo);
            conn.MySqlConnect();
            MySqlDataReader res = conn.mysql_query(sql);
            while (res.Read())
            {
                pagesID.Add(res.GetString(0));
                pagesName.Add(res.GetString(1));
                page_list.Items.Add(res.GetString(1));
            }
            page_list.Text = page_list.Items[0].ToString();
            page_list.SelectedIndex = 0;
            res.Close();
        }

        private void SearchForm_Shown(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (keywordBox.Text.Length == 0)
            {
                MessageBox.Show("請輸入要查詢的關鍵詞");
                return;
            }
            searchButton.Text = "資料查詢中…";
            searchButton.Enabled = false;
            pageName.Clear();
            postMessage.Clear();
            commentMessage.Clear();
            allCommentMessage.Clear();
            levelOneList.Items.Clear();
            commentsList.Items.Clear();
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            string sql = "select id,name from pages_info";
            MySqlDataReader res = conn.mysql_query(sql);
            while (res.Read())
            {
                pageName.Add(res.GetString(0), res.GetString(1));
            }
            res.Close();

            string keyword = keywordBox.Text.Replace("\"", "");

            string allPostID = "";
            keyword = keyword.Replace("%", "\\%");
            keyword = keyword.Replace("_", "\\_");
            sql = "select post_id, page_id, from_name, message,likes,comments,created_time, from_id from pages_posts where message like \"%" + keyword + "%\" and page_id in (select id from pages_info)";
            //MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
            //thecmd.Parameters.AddWithValue("?keyword", keywordBox.Text);
            //res = thecmd.ExecuteReader();
            res = conn.mysql_query(sql);
            Console.WriteLine(sql);
            /*
            if (res != null)
            {
                while (res.Read())
                {
                    string[] postData = { res.GetString(1), res.GetString(2), res.GetString(3), res.GetString(4), res.GetString(5), res.GetString(6) };
                    //page_id, from_name, message, likes, comments, created_time
                    postMessage.Add(res.GetString(0), postData);
                    allPostID += "\"" + res.GetString(0) + "\",";
                }
            }
            */
            while (res.Read())
            {
                string[] postData = { res.GetString(1), res.GetString(2), res.GetString(3), res.GetString(4), res.GetString(5), res.GetDateTime(6).ToString("yyyy-MM-dd HH:mm:ss"), res.GetString(7) };
                //page_id, from_name, message, likes, comments, created_time
                postMessage.Add(res.GetString(0), postData);
                allPostID += "\"" + res.GetString(0) + "\",";
            }
            res.Close();
            if (allPostID.Length > 0)
            {
                allPostID = allPostID.Substring(0, allPostID.Length - 1);
                sql = "select comment_id, post_id, page_id, from_name, message, likes, created_time,from_id from pages_posts_comments where post_id in (" + allPostID + ")";
                res = conn.mysql_query(sql);
                if (res != null)
                {
                    while (res.Read())
                    {
                        string[] commentData = { res.GetString(1), res.GetString(2), res.GetString(3), res.GetString(4), res.GetString(5), res.GetString(6), res.GetString(7) };
                        allCommentMessage.Add(res.GetString(0), commentData);
                    }
                    res.Close();
                }
            }
            //thecmd.Parameters.Clear();


            sql = "select comment_id, post_id, page_id, from_name, message, likes, created_time, from_id from pages_posts_comments where message like \"%" + keyword + "%\" and page_id in (select id from pages_info)";
            //MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
            //thecmd.Parameters.AddWithValue("?keyword", keywordBox.Text);
            //res = thecmd.ExecuteReader();
            res = conn.mysql_query(sql);
            if (res != null)
            {
                while (res.Read())
                {
                    string[] commentData = { res.GetString(1), res.GetString(2), res.GetString(3), res.GetString(4), res.GetString(5), res.GetString(6), res.GetString(7) };
                    //post_id, page_id, from_name, message, likes, created_time
                    commentMessage.Add(res.GetString(0), commentData);
                }
                res.Close();
            }
            //thecmd.Parameters.Clear();

            display_Message();

            searchButton.Enabled = true;
            searchButton.Text = "查詢";
        }
        private void display_Message()
        {
            levelOneList.Items.Clear();
            commentsList.Items.Clear();
            detailMessage.Clear();
            nameListBox.Items.Clear();
            nameListBox.DisplayMember = "Value";
            Dictionary<string, string> nameListDic = new Dictionary<string, string>();
            
            int counter = 0;
            if (postMessage != null && searchPostCheckBox.Checked)
            {
                foreach (KeyValuePair<string, string[]> kvp in postMessage)
                {
                    ListViewItem item = new ListViewItem(pageName[kvp.Value[0]]);
                    item.SubItems.Add(postString);
                    item.SubItems.Add(kvp.Value[2]);
                    string[] part = kvp.Value[5].Split('T');
                    if (filterWithTimeCheck.Checked)
                    {
                        DateTime startDate = Convert.ToDateTime(filterStartDate.Value.ToString("yyyy-MM-dd"));
                        DateTime endDate = Convert.ToDateTime(filterEndDate.Value.ToString("yyyy-MM-dd"));
                        //string startDate = filterStartDate.Value.Year.ToString() + "-" + filterStartDate.Value.Month.ToString("00") + "-" + filterStartDate.Value.Day.ToString("00");
                        //string endDate = filterEndDate.Value.Year.ToString() + "-" + filterEndDate.Value.Month.ToString("00") + "-" + filterEndDate.Value.Day.ToString("00");
                        //if (part[0].CompareTo(startDate) == -1 || part[0].CompareTo(endDate) == 1)
                        if (Convert.ToDateTime(part[0]).CompareTo(startDate) == -1 || Convert.ToDateTime(part[0]).CompareTo(endDate) == 1)
                        {
                            continue;
                        }
                    }
                    if (single_page.Checked)
                    {
                        string selectPageID = pagesID[page_list.SelectedIndex];
                        if (!kvp.Value[0].Equals(selectPageID))
                        {
                            continue;
                        }
                    }
                    item.SubItems.Add(part[0]);
                    item.SubItems.Add(kvp.Value[3]);
                    item.SubItems.Add(kvp.Value[4]);
                    item.SubItems.Add(kvp.Key);
                    levelOneList.Items.Add(item);
                    string theKey = kvp.Value[6];
                    string theValue = kvp.Value[1] + "(" + kvp.Value[5] + ")";
                    if (!nameListDic.ContainsKey(theKey))
                    {
                        nameListDic.Add(theKey, theValue);
                    }
                    counter++;
                }
            }
            if (commentMessage != null && searchCommentCheckBox.Checked)
            {
                foreach (KeyValuePair<string, string[]> kvp in commentMessage)
                {
                    ListViewItem item = new ListViewItem(pageName[kvp.Value[1]]);
                    item.SubItems.Add(commentString);
                    item.SubItems.Add(kvp.Value[3]);
                    string[] part = kvp.Value[5].Split('T');
                    if (filterWithTimeCheck.Checked)
                    {
                        string startDate = filterStartDate.Value.Year.ToString() + "-" + filterStartDate.Value.Month.ToString("00") + "-" + filterStartDate.Value.Day.ToString("00");
                        string endDate = filterEndDate.Value.Year.ToString() + "-" + filterEndDate.Value.Month.ToString("00") + "-" + filterEndDate.Value.Day.ToString("00");
                        if (part[0].CompareTo(startDate) == -1 || part[0].CompareTo(endDate) == 1)
                        {
                            continue;
                        }
                    }
                    if (single_page.Checked)
                    {
                        string selectPageID = pagesID[page_list.SelectedIndex];
                        if (!kvp.Value[1].Equals(selectPageID))
                        {
                            continue;
                        }
                    }
                    item.SubItems.Add(part[0]);
                    item.SubItems.Add(kvp.Value[4]);
                    item.SubItems.Add("0");
                    item.SubItems.Add(kvp.Key);
                    levelOneList.Items.Add(item);
                    string theKey = kvp.Value[6];
                    string theValue = kvp.Value[2] + "(" + kvp.Value[5] + ")";
                    if (!nameListDic.ContainsKey(theKey))
                    {
                        nameListDic.Add(theKey, theValue);
                    }
                    counter++;
                }
            }
            foreach (KeyValuePair<string, string> kvp in nameListDic)
            {
                nameListBox.Items.Add(kvp);
            }
            counterLabel.Text = counter.ToString() + "則訊息";
            nameOfGroup.Text = keywordBox.Text;
            selectAllNameBox.Checked = false;
        }
        private void levelOneList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            postSort[e.Column] ^= true;
            //showMessage.AppendText(resultBool[e.Column].ToString() + "\n");
            if(e.Column==4 || e.Column==5)
                levelOneList.ListViewItemSorter = new MyListViewSorter(e.Column, postSort[e.Column], true);
            else
                levelOneList.ListViewItemSorter = new MyListViewSorter(e.Column, postSort[e.Column], false);
            levelOneList.Sort();
        }

        private void commentsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            commentSort ^= true;
            commentsList.ListViewItemSorter = new MyListViewSorter(e.Column, commentSort, false);
            commentsList.Sort();
        }

        private void levelOneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (levelOneList.SelectedItems.Count == 0)
                return;
            detailMessage.Clear();
            string pageID = "";
            string fromName = "";
            string createdTime = "";
            string theMessage = "";
            string likes = "0";
            string commentsNumber = "0";
            int selectedIndex = levelOneList.SelectedItems[0].Index;
            ListViewItem selectedItem = levelOneList.Items[selectedIndex];
            //檢查第二欄(類型) 是否為Post
            if (selectedItem.SubItems[1].Text.Equals(postString))
            {//Item: (0) pageName, (1) Type, (2) message, (3) ID
                //PostData: (0) page_id, (1) from_name, (2) message, (3) likes, (4) comments, (5) created_time
                commentsList.Items.Clear();
                pageID = postMessage[selectedItem.SubItems[IDIndex].Text][0];
                fromName = postMessage[selectedItem.SubItems[IDIndex].Text][1];
                theMessage = postMessage[selectedItem.SubItems[IDIndex].Text][2];
                likes = postMessage[selectedItem.SubItems[IDIndex].Text][3];
                commentsNumber = postMessage[selectedItem.SubItems[IDIndex].Text][4];
                createdTime = postMessage[selectedItem.SubItems[IDIndex].Text][5];
                commentsList.Enabled = true;
                foreach (KeyValuePair<string, string[]> kvp in allCommentMessage)
                {
                    if (kvp.Value[0].Equals(selectedItem.SubItems[IDIndex].Text))
                    {
                        ListViewItem item = new ListViewItem(kvp.Value[3]);
                        item.SubItems.Add(kvp.Key);
                        commentsList.Items.Add(item);
                    }
                }
            }
            else
            {
                pageID = commentMessage[selectedItem.SubItems[IDIndex].Text][1];
                fromName = commentMessage[selectedItem.SubItems[IDIndex].Text][2];
                theMessage = commentMessage[selectedItem.SubItems[IDIndex].Text][3];
                likes = commentMessage[selectedItem.SubItems[IDIndex].Text][4];
                createdTime = commentMessage[selectedItem.SubItems[IDIndex].Text][5];
                commentsNumber = "N/A";
                commentsList.Items.Clear();
                commentsList.Enabled = false;
            }
            detailMessage.AppendText("粉絲頁：" + pageName[pageID] + "\n");
            detailMessage.AppendText("發表人：" + fromName + "\n");
            detailMessage.AppendText("建立時間：" + createdTime + "\n");
            detailMessage.AppendText("被按讚數：" + likes + "\n");
            detailMessage.AppendText("回應人數：" + commentsNumber + "\n");
            detailMessage.AppendText("訊息內容：" + theMessage);
        }

        private void commentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (commentsList.SelectedItems.Count == 0)
                return;
            detailMessage.Clear();
            string pageID = "";
            string fromName = "";
            string createdTime = "";
            string theMessage = "";
            string likes = "0";
            int selectedIndex = commentsList.SelectedItems[0].Index;
            ListViewItem selectedItem = commentsList.Items[selectedIndex];
            pageID = allCommentMessage[selectedItem.SubItems[1].Text][1];
            fromName = allCommentMessage[selectedItem.SubItems[1].Text][2];
            theMessage = allCommentMessage[selectedItem.SubItems[1].Text][3];
            likes = allCommentMessage[selectedItem.SubItems[1].Text][4];
            createdTime = allCommentMessage[selectedItem.SubItems[1].Text][5];
            
            detailMessage.AppendText("粉絲頁：" + pageName[pageID] + "\n");
            detailMessage.AppendText("發表人：" + fromName + "\n");
            detailMessage.AppendText("建立時間：" + createdTime + "\n");
            detailMessage.AppendText("被按讚數：" + likes + "\n");
            
            detailMessage.AppendText("訊息內容：" + theMessage);
            levelOneList.SelectedItems.Clear();
        }
        private void levelOneList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = levelOneList.Columns[e.ColumnIndex].Width;
        }

        private void commentsList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = commentsList.Columns[e.ColumnIndex].Width;
        }

        private void filterWithTimeCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (filterWithTimeCheck.Checked)
            {
                filterStartDate.Enabled = true;
                filterEndDate.Enabled = true;
            }
            else
            {
                filterStartDate.Enabled = false;
                filterEndDate.Enabled = false;
            }
            //display_Message();
        }

        private void searchPostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //display_Message();
        }

        private void searchCommentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //display_Message();
        }

        private void filterStartDate_CloseUp(object sender, EventArgs e)
        {
            //display_Message();
        }

        private void filterEndDate_CloseUp(object sender, EventArgs e)
        {
            //display_Message();
        }

        private void DisaplayMessage_Click(object sender, EventArgs e)
        {
            display_Message();
        }

        private void single_page_CheckedChanged(object sender, EventArgs e)
        {
            page_list.Enabled = single_page.Checked;
        }

        private void createGroup_Click(object sender, EventArgs e)
        {
            if (nameListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show("請先選擇要群組的名單");
                return;
            }
            if (nameOfGroup.Text.Length == 0)
            {
                MessageBox.Show("請輸入要群組的名稱");
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("insert ignore into talk_msg_namelist (id,category) values ");
            foreach (KeyValuePair<string, string> kvp in nameListBox.CheckedItems)
            {
                sb.Append("(" + kvp.Key + ",?category),");
            }

            string sql = sb.ToString();
            sql = sql.Remove(sql.Length - 1);
            DBConnection conn = new DBConnection(dbInfo);
            conn.MySqlConnect();
            MySqlCommand thecmd = new MySqlCommand(sql, conn.conn);
            thecmd.Parameters.AddWithValue("?category", nameOfGroup.Text);
            MySqlDataReader themyData = thecmd.ExecuteReader();
            themyData.Close();
            thecmd.Parameters.Clear();
            conn.closeMySqlConnection();
            MessageBox.Show("『" + nameOfGroup.Text + "』群組已建立成功");
        }

        private void selectAllNameBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAllNameBox.Checked)
            {
                for (int i = 0; i < nameListBox.Items.Count; i++)
                {
                    nameListBox.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < nameListBox.Items.Count; i++)
                {
                    nameListBox.SetItemChecked(i, false);
                }
            }
            checkedCountLabel.Text = nameListBox.CheckedItems.Count.ToString();
        }

        private void nameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedCountLabel.Text = nameListBox.CheckedItems.Count.ToString();
        }
    }
    public class MyListViewSorter : IComparer
    {
        private int _columnIndex;
        bool orderByDesc;
        bool number;
        public MyListViewSorter(int columnIndex, bool orderByDesc, bool number)
        {
            _columnIndex = columnIndex;
            this.orderByDesc = orderByDesc;
            this.number = number;
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            ListViewItem lvi1;
            ListViewItem lvi2;
            if (orderByDesc)
            {
                lvi1 = x as ListViewItem;
                lvi2 = y as ListViewItem;
            }
            else
            {
                lvi2 = x as ListViewItem;
                lvi1 = y as ListViewItem;
            }
            if (number)
            {
                return Int32.Parse(lvi1.SubItems[_columnIndex].Text).CompareTo(Int32.Parse(lvi2.SubItems[_columnIndex].Text));
            }
            else
            {
                return lvi1.SubItems[_columnIndex].Text.CompareTo(lvi2.SubItems[_columnIndex].Text);
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace StandaloneNode
{
    class DBConnection
    {
        public MySqlConnection conn;
        private List<string> connectionInfo;
        public DBConnection(List<string> connectionInfo)
        {
            this.connectionInfo = connectionInfo;
        }
        public bool checkDBExist()
        {
            this.connectionInfo = new List<string>();
            this.connectionInfo.Add("localhost");
            this.connectionInfo.Add("enricolu");
            this.connectionInfo.Add("111platform!");
            this.connectionInfo.Add("networkvis");

            string dbHost = connectionInfo.ElementAt<string>(0);
            string dbUser = connectionInfo.ElementAt<string>(1);
            string dbPass = connectionInfo.ElementAt<string>(2);
            string dbName = connectionInfo.ElementAt<string>(3);
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass;// +";CharSet=utf8_general_ci";// +";CharSet=utf8mb4";
            conn = new MySqlConnection(connStr);

            // 連線到資料庫 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                conn = null;
                return false;
            }
            catch
            {
                return false;
            }
            string sql = "select count(*) from information_schema.tables where table_schema=\"" + dbName + "\"";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader res = cmd.ExecuteReader();
            sql = "CREATE SCHEMA IF NOT EXISTS `" + dbName +"`";// +"` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`access_token` ( `access_token` varchar(300) NOT NULL, PRIMARY KEY (`access_token`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_posts_likes` ( `post_id` varchar(50) NOT NULL, `id` varchar(30) NOT NULL, `name` varchar(100) default NULL, `post_created_time` datetime default NULL, `page_id` varchar(45) default NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`post_id`,`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_posts_comments` ( `comment_id` varchar(60) NOT NULL, `page_id` varchar(50) NOT NULL, `post_id` varchar(50) NOT NULL, `from_id` varchar(30) default NULL, `from_name` varchar(100) default NULL, `message` text, `created_time` datetime default NULL, `post_created_time` datetime default NULL, `likes` int(11) NOT NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`comment_id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            //sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_posts` ( `post_id` varchar(50) NOT NULL, `page_id` varchar(50) NOT NULL, `from_id` varchar(30) default NULL, `from_name` varchar(100) default NULL, `message` text CHARACTER SET utf8mb4, `picture` varchar(700) default NULL, `link` varchar(700) default NULL, `name` varchar(500) NOT NULL default '', `caption` text, `source` varchar(300) default NULL, `icon` varchar(300) default NULL, `type` varchar(30) NOT NULL, `object_id` varchar(30) NOT NULL, `description` text, `likes` int(10) unsigned NOT NULL default '0', `created_time` datetime default NULL, `updated_time` datetime default NULL, `comments` int(10) unsigned NOT NULL default '0', `shares` int(11) NOT NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`post_id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_posts` ( `post_id` varchar(50) NOT NULL, `page_id` varchar(50) NOT NULL, `from_id` varchar(30) default NULL, `from_name` varchar(100) default NULL, `message` text CHARACTER SET utf8, `picture` varchar(700) default NULL, `link` varchar(700) default NULL, `name` varchar(500) NOT NULL default '', `caption` text, `source` varchar(300) default NULL, `icon` varchar(300) default NULL, `type` varchar(30) NOT NULL, `object_id` varchar(30) NOT NULL, `description` text, `likes` int(10) unsigned NOT NULL default '0', `created_time` datetime default NULL, `updated_time` datetime default NULL, `comments` int(10) unsigned NOT NULL default '0', `shares` int(11) NOT NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`post_id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_info` ( `id` varchar(30) NOT NULL, `name` varchar(300) NOT NULL, `picture` varchar(300) NOT NULL, `link` varchar(300) NOT NULL, `category` varchar(300) NOT NULL, `website` varchar(300) NOT NULL, `founded` varchar(100) NOT NULL, `info` text NOT NULL, `likes` int(11) NOT NULL, `talking_about_count` int(11) NOT NULL, `feeds` int(11) NOT NULL, `comments` int(11) NOT NULL, `updatetime` datetime NOT NULL, `new_id` int(11) NOT NULL, `timespent` varchar(100) NOT NULL, `iiicategory` text NOT NULL, PRIMARY KEY (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_fans` ( `pages_id` varchar(30) NOT NULL, `id` varchar(30) NOT NULL, `name` varchar(100) NOT NULL, PRIMARY KEY (`pages_id`,`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`pages_logs` ( `id` varchar(30) NOT NULL, `name` varchar(100) NOT NULL, `picture` varchar(300) NOT NULL, `link` varchar(300) NOT NULL, `category` varchar(300) NOT NULL, `website` varchar(300) NOT NULL, `founded` varchar(100) NOT NULL, `info` text NOT NULL, `likes` int(11) NOT NULL, `talking_about_count` int(11) NOT NULL, `feeds` int(11) NOT NULL, `comments` int(11) NOT NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`id`,`updatetime`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`user_info` ( `id` varchar(30) NOT NULL,`name` varchar(100) NOT NULL,`username` varchar(100) NOT NULL, `gender` varchar(10) NOT NULL, `updatetime` datetime NOT NULL, PRIMARY KEY (`id`), KEY `updatetime` (`updatetime`)) ENGINE=MyISAM DEFAULT CHARSET=utf8";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`talk_msg_msg` ( `id` int(11) NOT NULL, `msg` text NOT NULL, PRIMARY KEY (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`talk_msg_namelist` ( `id` varchar(30) NOT NULL, `category` varchar(100) NOT NULL, PRIMARY KEY (`id`, `category`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`talk_msg_user_log` ( `ID` varchar(50) NOT NULL, `Msg` text NOT NULL, `SendTime` DateTime NOT NULL, `SendComplete` tinyint(1) NOT NULL, PRIMARY KEY (`ID`, `SendTime`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`talk_msg_mail_sendstatus` ( `MailAcc` varchar(50) NOT NULL, `SendTime` DateTime NOT NULL, `SendCount` int(10) NOT NULL, PRIMARY KEY (`MailAcc`, `SendTime`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            sql = "CREATE TABLE IF NOT EXISTS " + dbName + ".`talk_msg_account` ( `Acc` varchar(50) NOT NULL, `FbPw` varchar(50) NOT NULL, `MailPw` varchar(50) NOT NULL, PRIMARY KEY (`Acc`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            cmd.CommandText = sql;
            cmd.ExecuteReader().Close();

            try
            {
                conn.Close();
            }
            catch { }
            return true;
        }
        public string MySqlConnect()
        {
            string dbHost = connectionInfo.ElementAt<string>(0);
            string dbUser = connectionInfo.ElementAt<string>(1);
            string dbPass = connectionInfo.ElementAt<string>(2);
            string dbName = connectionInfo.ElementAt<string>(3);
            //Console.WriteLine(dbHost + "-" + dbUser);
            // 如果有特殊的編碼在database後面請加上;CharSet=編碼, utf8請使用utf8_general_ci 
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;// +";CharSet=utf8_general_ci"; ;// ";CharSet=utf8mb4;";
            conn = new MySqlConnection(connStr);

            // 連線到資料庫 
            try
            {
                conn.Open();
                return "";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                conn = null;
                switch (ex.Number)
                {
                    case 0:
                        return "無法連線。";
                    case 1045:
                        return "使用者帳號或密碼錯誤，請再試一次。";
                    case 1049:
                        return dbName + "不存在，請再試一次。";
                    default:
                        return "發生不明連線錯誤。";
                }
            }
        }
        public MySqlDataReader mysql_query(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.CommandTimeout = 120000;
                MySqlDataReader myData = cmd.ExecuteReader();
                return myData;
            }
            catch (Exception ex)
            {
                //mydata.AppendText( sql + "\r\nError " + ex.Number + " : " + ex.Message);
                Console.WriteLine("錯啦  " + ex.ToString());
            }
            return null;
        }
        public void closeMySqlConnection()
        {
            try
            {
                conn.Close();
            }
            catch (Exception closeException)
            {
                Console.WriteLine(closeException.ToString());
            }
        }
    }
}
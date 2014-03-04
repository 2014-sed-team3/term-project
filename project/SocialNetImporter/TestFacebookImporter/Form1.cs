using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace TestFacebookImporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeFile();
            //readXML();
        }

        private void readXML()
        {
            TextReader tr = new StreamReader("GraphAsGraphML.txt");
            string data = tr.ReadToEnd();
            tr.Close();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("gml", "http://graphml.graphdrawing.org/xmlns");

            

            XmlNodeList nodes = doc.SelectNodes("descendant::gml:node",nsmgr);

            int[,] matrix = new int[nodes.Count,nodes.Count];

            XmlNodeList edges = doc.SelectNodes("descendant::gml:edge", nsmgr);

            foreach (XmlNode edge in edges)
            {
                matrix[getIndex(edge.Attributes["source"].InnerText,nodes),getIndex(edge.Attributes["target"].InnerText,nodes)]=1;
                matrix[getIndex(edge.Attributes["target"].InnerText, nodes), getIndex(edge.Attributes["source"].InnerText, nodes)] = 1;
            }

            TextWriter tw = new StreamWriter("friends.txt");

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    tw.Write(matrix[i,j]);                    
                    tw.Write(" ");
                }
                tw.WriteLine();
            }

            tw.Close();
           
        }

        private int getIndex(string node, XmlNodeList nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (((XmlNode)nodes[i]).Attributes["id"].InnerText.Equals(node))
                {
                    return i;
                }
            }
            return -1;
        }

        private void writeFile()
        {
            Smrf.NodeXL.GraphDataProviders.Facebook.FacebookGraphDataProvider fbGraph = new Smrf.NodeXL.GraphDataProviders.Facebook.FacebookGraphDataProvider();
            string data="";
            
            try
            {
                fbGraph.TryGetGraphData(out data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n Please copy the following information and paste it to http://socialnetimporter.codeplex.com/discussions :\n" + e.StackTrace);
            }
            TextWriter tw = new StreamWriter("GraphASGraphML.txt");


            // write a line of text to the file
            tw.WriteLine(data);

            // close the stream
            tw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Smrf.NodeXL.GraphDataProviders.Facebook.FacebookFanPageGraphDataProvider fbGraph = new Smrf.NodeXL.GraphDataProviders.Facebook.FacebookFanPageGraphDataProvider();
            string data;            
            fbGraph.TryGetGraphDataAsTemporaryFile(out data);
            TextWriter tw = new StreamWriter("FanPageASGraphML.txt");
            // write a line of text to the file
            tw.WriteLine(data);

            // close the stream
            tw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Smrf.NodeXL.GraphDataProviders.Facebook.FacebookGroupGraphDataProvider fbGraph = new Smrf.NodeXL.GraphDataProviders.Facebook.FacebookGroupGraphDataProvider();
            string data;
            fbGraph.TryGetGraphDataAsTemporaryFile(out data);
            TextWriter tw = new StreamWriter("FanPageASGraphML.txt");
            // write a line of text to the file
            tw.WriteLine(data);

            // close the stream
            tw.Close();
        }
    }
}

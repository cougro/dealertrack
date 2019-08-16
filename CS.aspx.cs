using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Text.RegularExpressions;

public partial class CS : System.Web.UI.Page
{
    protected void ImportCSV(object sender, EventArgs e)
    {
        //Upload and save the file
        string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
        FileUpload1.SaveAs(csvPath);

        //Create a DataTable.
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[6] { new DataColumn("DealNumber", typeof(string)),
        new DataColumn("CustomerName", typeof(string)),
        new DataColumn("DealershipName", typeof(string)),
        new DataColumn("Vehicle", typeof(string)),
        new DataColumn("Price",typeof(string)), 
        new DataColumn("Date",typeof(string))});

        try
        {
            using (CsvReader reader = new CsvReader(csvPath))
            {
                var bIsFirstLine = true;

                foreach (string[] values in reader.RowEnumerator)
                {
                    int i = 0;
                    // First line of CSV is just headers of the columns which we already have with the DataColumn headers already added, so we skip it
                    if (bIsFirstLine)
                    {
                        bIsFirstLine = false;
                        continue;
                    }
                    else
                        dt.Rows.Add();

                    var prefix = "CAD$"; // add this prefix if item is under "Price" datacolumn

                    foreach (string cell in values)
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = (i == 4) ? prefix + cell : cell;
                        i++;
                    }

                }

            }

            //Bind the DataTable.
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch
        {
            File.Delete(csvPath);
            Response.Write("<script>alert('Not a valid CSV file. Try again')</script>");
        }
    }

    // Helper Class
    public sealed class CsvReader : System.IDisposable
    {
        public CsvReader(string fileName)
            : this(new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        public CsvReader(Stream stream)
        {
            __reader = new StreamReader(stream);
        }

        public System.Collections.IEnumerable RowEnumerator
        {
            get
            {
                if (null == __reader)
                    throw new System.ApplicationException("I can't start reading without CSV input.");

                __rowno = 0;
                string sLine;
                string sNextLine;

                while (null != (sLine = __reader.ReadLine()))
                {
                    while (rexRunOnLine.IsMatch(sLine) && null != (sNextLine = __reader.ReadLine()))
                        sLine += "\n" + sNextLine;

                    __rowno++;
                    string[] values = rexCsvSplitter.Split(sLine);

                    for (int i = 0; i < values.Length; i++)
                        values[i] = Csv.Unescape(values[i]);

                    yield return values;
                }

                __reader.Close();
            }
        }

        public long RowIndex { get { return __rowno; } }

        public void Dispose()
        {
            if (null != __reader) __reader.Dispose();
        }

        //============================================


        private long __rowno = 0;
        private TextReader __reader;
        private static Regex rexCsvSplitter = new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
        private static Regex rexRunOnLine = new Regex(@"^[^""]*(?:""[^""]*""[^""]*)*""[^""]*$");
    }

    /// <summary>
    /// Helper Class
    /// </summary>
    public static class Csv
    {
        public static string Escape(string s)
        {
            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
                s = QUOTE + s + QUOTE;

            return s;
        }

        public static string Unescape(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }


        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };
    }
}
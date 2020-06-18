using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using iTextSharp.text.pdf;
using iTextSharp.text.xml;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using CadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;

[assembly: CommandClass(typeof(PDFtoDWG.Class1))]

namespace PDFtoDWG
{
    public class Class1
    {
        /*
         * Main combines and calls functions based on input from a printing range dialog box
         * Calls: GetFilepathFromUser, GetPageStartFromUser, GetPageEndFromUser, GetSaveNameFromUser and ImportPdf
         */
        [CommandMethod("PDFtoDWG")]
        public void PDFtoDWG()
        {
            string userfile, usersave;  // declarations
            int userpage, pageend;

            //Set FILEDIA to 0
            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("FILEDIA", 0);
            userfile = GetFilepathFromUser();  // call these functions to get filepath and how many pages in pdf from user
            var confirmResult = MessageBox.Show("Do you want to import all pages? NOTE: If you select NO, it will prompt you for the starting and ending page.", "Importing Page Range", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                userpage = 1;
                string ppath = userfile;
                PdfReader pdfReader = new PdfReader(userfile);
                int numberOfPages = pdfReader.NumberOfPages;
                Console.WriteLine(numberOfPages);
                Console.ReadLine();
                usersave = GetSaveNameFromUser();
                ImportPdf(userfile, userpage, usersave, numberOfPages);
            }
            else
            {
                userpage = GetPageStartFromUser(); 
                pageend = GetPageEndFromUser();
                usersave = GetSaveNameFromUser();
                ImportPdf(userfile, userpage, usersave, pageend);
            }
        }


        /*
         * NewDrawing: creates a new drawing using template acad.dwt
         * Calls: none
         * Called by: ImportPdf
         * Returns: new drawing it created
         */
        // [CommandMethod("NewDrawing")]
        public Document NewDrawing()
        {
            // Specify the template to use, if the template is not found
            // the default settings are used.
            string strTemplatePath = "acad.dwt";

            DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
            Document acDoc = acDocMgr.Add(strTemplatePath);

            acDocMgr.MdiActiveDocument = acDoc;
            return acDoc; 
        }


        /*
         * Opens dialog for user to select filepath of pdf he wants to convert
         * Calls: none
         * Called by: Main
         * Returns: filepath of file selected in dialog to string in Main called userfile
         */
        // [CommandMethod("GetStringFromUser")]
        public string GetFilepathFromUser()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            OpenFileDialog filesearch = new OpenFileDialog();
            if (filesearch.ShowDialog() == DialogResult.OK)
            {
                string strFileName = filesearch.FileName;
                return strFileName;
            }
            return " ";
        }


        /*
        * Opens a dialog for user to select save folder and then prompts user for string to input savename
        * Calls: none
        * Called by: Main
        * Returns: filepath of folder selected in dialog + savename entered by user string in Main called usersave
        */
        // [CommandMethod("GetStringFromUser")]
        public string GetSaveNameFromUser()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            string strFileName, strFolderName;

            FolderBrowserDialog foldersearch = new FolderBrowserDialog();
            foldersearch.RootFolder = Environment.SpecialFolder.Desktop;
            foldersearch.Description = "SELECT FOLDER YOU WANT TO SAVE TO";

            if (foldersearch.ShowDialog() == DialogResult.OK)
            {
                strFolderName = foldersearch.SelectedPath.ToString();

                PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter the name you would like to save the file as: ");

                pStrOpts.AllowSpaces = true;
                PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);
                strFileName = strFolderName +"\\"+ pStrRes.StringResult;
                return strFileName;
            }
            return " ";
        }


        /* Waits for user to put in integer input of page count of start page of pdf he wants to convert
         * Calls: none
         * Called by: Main
         * Returns: page start value to integer in Main called userpage if printing range
         */
        // [CommandMethod("GetIntegerFromUser")]
        public int GetPageStartFromUser()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("\nEnter the Start page of the PDF you would like to convert to DWG: ");
            PromptIntegerResult pIntRes = acDoc.Editor.GetInteger(pIntOpts);

            return pIntRes.Value;
        }


        /* Waits for user to put in integer input of page count of end page of pdf he wants to convert
        * Calls: none
        * Called by: Main
        * Returns: page end value to integer in Main called pageend if printing range
        */
        // [CommandMethod("GetIntegerFromUser")]
        public int GetPageEndFromUser()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("\nEnter the End page of the PDF you would like to convert to DWG: ");
            PromptIntegerResult pIntRes = acDoc.Editor.GetInteger(pIntOpts);

            return pIntRes.Value;
        }


        /* Saves the active drawing which was a pdf to a dwg format in usersave1, usersave2, etc.
         * Calls: none
         * Called by: PdfImport
         * Returns: none
         */
        // [CommandMethod("SaveActiveDrawing")]
        public string SaveActiveDrawing(string usersave, int page, Document acDoc)
        {
            string strDWGName = acDoc.Name;

            object obj = Autodesk.AutoCAD.ApplicationServices.Application.GetSystemVariable("DWGTITLED");

            // Check to see if the drawing has been named
            if (System.Convert.ToInt16(obj) == 0)
            {
                // If the drawing is using a default name (Drawing1, Drawing2, etc)
                // then provide a new name
                strDWGName = usersave + page + ".dwg";
            }
            return strDWGName;
        }


        /* Sends the pdf into AutoCAD and runs -PDFIMPORT then quits the program
         * Calls: NewDrawing, SaveActiveDrawing
         * Called by: Main
         * Returns: none
         */
        //[CommandMethod("PdfImport")]
        public void ImportPdf(string userfile, int userpage, string usersave, int pageend)
        {
            var dwg = CadApp.DocumentManager.MdiActiveDocument;
            //var ed = dwg.Editor;
            int fileDia = Convert.ToInt32(CadApp.GetSystemVariable("FILEDIA"));
            string save;

            for (int page = userpage; page <= pageend; page++)
            {
                //Start command -PDFIMPORT with pdf file name supplied
                var cmd = "-PDFIMPORT f\r";
                dwg = NewDrawing();
                dwg.SendStringToExecute(cmd, true, false, false);
                dwg.SendStringToExecute(userfile + "\r", true, false, false);
                // supply page input
                dwg.SendStringToExecute(page + "\r", true, false, false);
                // supply insertion location inputset
                dwg.SendStringToExecute("10.0,10.0\r", true, false, false);
                // supply scale input
                dwg.SendStringToExecute("1.0\r", true, false, false);
                // supply rotation input
                dwg.SendStringToExecute("0.0\r", true, false, false);
                save = SaveActiveDrawing(usersave, page, dwg);
                dwg.SendStringToExecute("SAVE\r", true, false, false);
                dwg.SendStringToExecute(save + "\r", true, false, false);
            }
            dwg.SendStringToExecute("_QUIT\r", true, false, false);
        }
    }
}

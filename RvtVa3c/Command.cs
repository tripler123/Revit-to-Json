#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Interop;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using DialogResult = System.Windows.Forms.DialogResult;
#endregion // Namespaces

namespace RvtVa3c
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Solución de ensamblado personalizado para encontrar nuestra 
        /// DLL de soporte sin tener que colocar toda nuestra aplicación
        /// en una subcarpeta del directorio de Revit.exe.
        /// </summary>
        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Newtonsoft"))
            {
                string filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                filename = Path.Combine(filename, "Newtonsoft.Json.dll");

                if (File.Exists(filename))
                {
                    return System.Reflection.Assembly.LoadFrom(filename);
                }
            }
            return null;
        }

        /// <summary>
        /// Exporte una vista 3D dada a JSON utilizando nuestro contexto exportador personalizado.
        /// </summary>
        public void ExportView3D(View3D view3d, string filename)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Document doc = view3d.Document;

            Va3cExportContext context = new Va3cExportContext(doc, filename);

            CustomExporter exporter = new CustomExporter(doc, context);

            // Nota: Excluir caras simplemente suprime las llamadas de OnFaceBegin, 
            //no el procesamiento real de la teselación de caras. Las mallas de las 
            //caras seguirán siendo recibidas por el contexto.
            //exporter.IncludeFaces = false; // eliminado en Revit 2017

            exporter.ShouldStopOnError = false;

            exporter.Export(view3d);
        }

        #region UI to Filter Parameters
        public static ParameterFilter _filter; // Form para filtrar los elementos
        public static TabControl _tabControl; //Pestañas para cada categoria dentro del form
        public static Dictionary<string, List<string>> _parameterDictionary; //Lista de Parametro por Categoria totales
        public static Dictionary<string, List<string>> _toExportDictionary; //Lista de Paramtro por Categoria filtrado

        /// <summary>
        /// Función para filtrar los parámetros de los objetos en la escena
        /// </summary>
        /// <param name="doc">Revit Document</param>
        /// <param name="includeType">Incluir parámetros de tipo en el diálogo de filtro</param>
        public void filterElementParameters(Document doc, bool includeType)
        {
            _parameterDictionary = new Dictionary<string, List<string>>();
            _toExportDictionary = new Dictionary<string, List<string>>();

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);

            // Crea un diccionario con todas las parametros para cada categoría.

            foreach (var fi in collector)
            {
                // fi --> Elemento

                string category = fi.Category.Name; //Categoria del elemento

                if (category != "Title Blocks" && category != "Generic Annotations" && category != "Detail Items" && category != "Cameras")
                {
                    IList<Parameter> parameters = fi.GetOrderedParameters(); //Todos los paramtro del elemento - Categoria
                    List<string> parameterNames = new List<string>(); //Lista para Almacenar los parametros

                    foreach (Parameter p in parameters)
                    {
                        string pName = p.Definition.Name; //Nombre del Parametros
                        string tempVal = ""; //Valor temporal

                        if (StorageType.String == p.StorageType) //Tipo = String
                        {
                            tempVal = p.AsString();
                        }
                        else //Tipo != String
                        {
                            tempVal = p.AsValueString();
                        }
                        if (!string.IsNullOrEmpty(tempVal)) //Si el valor temporar diferente a null o empty
                        {
                            if (_parameterDictionary.ContainsKey(category))
                            {
                                if (!_parameterDictionary[category].Contains(pName))
                                {
                                    _parameterDictionary[category].Add(pName);
                                }
                            }
                            else
                            {
                                parameterNames.Add(pName);
                            }
                        }
                    }
                    if (parameterNames.Count > 0)
                    {
                        _parameterDictionary.Add(category, parameterNames);
                    }
                    if (includeType)
                    {
                        ElementId idType = fi.GetTypeId();

                        if (ElementId.InvalidElementId != idType)
                        {
                            Element typ = doc.GetElement(idType);
                            parameters = typ.GetOrderedParameters();
                            List<string> parameterTypes = new List<string>();
                            foreach (Parameter p in parameters)
                            {
                                string pName = "Type " + p.Definition.Name;
                                string tempVal = "";
                                if (!_parameterDictionary[category].Contains(pName))
                                {
                                    if (StorageType.String == p.StorageType)
                                    {
                                        tempVal = p.AsString();
                                    }
                                    else
                                    {
                                        tempVal = p.AsValueString();
                                    }

                                    if (!string.IsNullOrEmpty(tempVal))
                                    {
                                        if (_parameterDictionary.ContainsKey(category))
                                        {
                                            if (!_parameterDictionary[category].Contains(pName))
                                            {
                                                _parameterDictionary[category].Add(pName);
                                            }
                                        }
                                        else
                                        {
                                            parameterTypes.Add(pName);
                                        }
                                    }
                                }
                            }

                            if (parameterTypes.Count > 0)
                            {
                                _parameterDictionary[category].AddRange(parameterTypes);
                            }
                        }

                    }
                }
            }

            // Create filter UI.

            _filter = new ParameterFilter();

            _tabControl = new TabControl();
            _tabControl.Size = new System.Drawing.Size(600, 375);
            _tabControl.Location = new System.Drawing.Point(0, 55);
            _tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            int j = 8;

            // Rellenar los parámetros como una casilla de verificación en cada pestaña
            foreach (string c in _parameterDictionary.Keys)
            {
                //Create a checklist
                CheckedListBox checkList = new CheckedListBox();

                //set the properties of the checklist
                checkList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                checkList.FormattingEnabled = true;
                checkList.HorizontalScrollbar = true;
                checkList.Items.AddRange(_parameterDictionary[c].ToArray());
                checkList.MultiColumn = true;
                checkList.Size = new System.Drawing.Size(560, 360);
                checkList.ColumnWidth = 200;
                checkList.CheckOnClick = true;
                checkList.TabIndex = j;
                j++;

                for (int i = 0; i <= (checkList.Items.Count - 1); i++)
                {
                    checkList.SetItemCheckState(i, CheckState.Checked);
                }

                //Agregar una pestaña
                TabPage tab = new TabPage(c);
                tab.Name = c;

                //adjuntar la lista de verificación a la pestaña
                tab.Controls.Add(checkList);

                // adjuntar la pestaña al control de pestañas
                _tabControl.TabPages.Add(tab);
            }

            // Adjunte el control de pestaña al formulario de filtro
            _filter.Controls.Add(_tabControl);

            // Mostrar el interfaz del filtro
            _filter.ShowDialog();

            // Pasa por cada pestaña y obtén los parámetros para exportar

            foreach (TabPage tab in _tabControl.TabPages)
            {
                List<string> parametersToExport = new List<string>();
                foreach (var checkedP in ((CheckedListBox)tab.Controls[0]).CheckedItems)
                {
                    parametersToExport.Add(checkedP.ToString()); //Se Agrega Los parametros
                }
                _toExportDictionary.Add(tab.Name, parametersToExport); //Categoria y Paramtros Exportar. Predefinir Estos Parametros.
            }

            #region
            _parameterDictionary.Clear();
            _toExportDictionary.Clear();
            List<string> listaCategoria = new List<string>
            {
                "Structural Columns",
                "Structural Framing",
                "Floor",
                "Structural Foundations"
            };

            List<string> listaParametros = new List<string>
            {
                "Elemento",
                "Nivel del Elemento",
                "Sector",
                "Structural Material",
                "Altura",
                "Perimetro",
                "Mark",
                "Volume"
            };
            if (_toExportDictionary.Count <= 0)
            {
                foreach (string cat in listaCategoria)
                {
                    //_parameterDictionary.Add(cat, listaParametros);
                    _toExportDictionary.Add(cat, listaParametros);
                }
            }
            


            #endregion


        }
        #endregion 

        #region SelectFile
        /// <summary>
        /// Almacene la última carpeta de salida seleccionada
        /// por el usuario en la sesión de edición actual.
        /// </summary>
        static string _output_folder_path = null;

        /// <summary>
        /// Devuelve true: el usuario selecciona y confirma
        /// el nombre y la carpeta del archivo de salida.
        /// </summary>
        static bool SelectFile( ref string folder_path, ref string filename)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "Select JSON Output File";
            dlg.Filter = "JSON files|*.js";

            if (null != folder_path && 0 < folder_path.Length)
            {
                dlg.InitialDirectory = folder_path;
            }

            dlg.FileName = filename;

            bool rc = DialogResult.OK == dlg.ShowDialog();

            if (rc)
            {
                filename = Path.Combine(dlg.InitialDirectory, dlg.FileName);
                folder_path = Path.GetDirectoryName(filename);
            }
            return rc;
        }
        #endregion 

        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Verifique que estamos en una vista 3D.

            View3D view = doc.ActiveView as View3D;

            if (null == view)
            {
                Util.ErrorMsg("Uste debe estar en una vista 3D para Exportar.");
                return Result.Failed;
            }

            // Solicitud de selección de nombre de archivo de salida.

            string filename = doc.PathName;

            if (0 == filename.Length)
            {
                filename = doc.Title;
            }

            if (null == _output_folder_path)
            {
                // A veces, el comando falla si el archivo 
                //se separa de la central y no se guarda localmente

                try
                {
                    _output_folder_path = Path.GetDirectoryName(filename);
                }
                catch
                {
                    TaskDialog.Show("Carpeta no encontrada", "Guarde el archivo y vuelva a ejecutar el comando");
                    return Result.Failed;
                }
            }

            filename = Path.GetFileName(filename) + ".js";

            if (!SelectFile(ref _output_folder_path, ref filename))
            {
                return Result.Cancelled;
            }

            filename = Path.Combine(_output_folder_path, filename);

            // Preguntar al usuario si elegir interactivamente 
            //qué parámetros exportar o simplemente exportarlos todos.

            TaskDialog td = new TaskDialog("Ask user to filter parameters");
            td.Title = "Filter parameters";
            td.CommonButtons = TaskDialogCommonButtons.No | TaskDialogCommonButtons.Yes;
            td.MainInstruction = "Do you want to filter the parameters of the objects to be exported?";
            td.MainContent = "Click Yes and you will be able to select parameters for each category in the next window";
            td.AllowCancellation = true;
            td.VerificationText = "Check this to include type properties";

            if (TaskDialogResult.Yes == td.Show())
            {
                filterElementParameters(doc, td.WasVerificationChecked());
                if (ParameterFilter.status == "cancelled")
                {
                    ParameterFilter.status = "";
                    return Result.Cancelled;
                }
            }

            // Save file.

            ExportView3D(doc.ActiveView as View3D, filename);

            return Result.Succeeded;
        }
    }
}

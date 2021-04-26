using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestNativeFilePicker : MonoBehaviour
{
    public TextMeshProUGUI uiDebugText;

    public Button uiButtonTestSingle;
    public Button uiButtonTestMultiple;

    public Button uiButtonTestExportASCII;
    public Button uiButtonTestExportBIN;

    public TMP_InputField uiInputUTI;

    // Start is called before the first frame update
    void Start()
    {
        string ext = "???";
        ext = NativeFilePicker.ConvertExtensionToFileType("obj");
        PrintDebug(ext);
        ext = NativeFilePicker.ConvertExtensionToFileType("fbx");
        PrintDebug(ext);
        ext = NativeFilePicker.ConvertExtensionToFileType("stl");
        PrintDebug(ext);
        ext = NativeFilePicker.ConvertExtensionToFileType("dae");
        PrintDebug(ext);

        uiButtonTestSingle.onClick.AddListener(TryIosPickerOneFile);
        uiButtonTestMultiple.onClick.AddListener(TryIosPickerMultipleFiles);
        uiButtonTestExportASCII.onClick.AddListener(TryIosExportASCII);
        uiButtonTestExportBIN.onClick.AddListener(TryIosExportBIN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TryIosPickerOneFile()
    {
        // Don't attempt to import/export files if the file picker is already open
        if (NativeFilePicker.IsFilePickerBusy())
        {
            return;
        }

        string importFilePath = null;
        string[] extensions = new string[1];
        if (uiInputUTI.text.Length > 1)
        {
            extensions[0] = uiInputUTI.text;
        }
        else
        {
            extensions[0] = "public.item";
        }

        // Pick any file
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                PrintDebug("Operation cancelled");
            }
            else
            {
                PrintDebug("Picked file: " + path);
                importFilePath = path;
            }
        }, extensions);

        PrintDebug("Permission result: " + permission);
        PrintDebug("Does file exist??: " + File.Exists(importFilePath));
        
        if(importFilePath != null)
        {
            string[] stringSplit = importFilePath.Split(new char[] { '.' });
            if(stringSplit.Length >= 2)
            {
                if(stringSplit[1] == "txt" && File.Exists(importFilePath))
                {
                    PrintDebug(File.ReadAllText(importFilePath));
                }
            }
        }
    }

    private void TryIosPickerMultipleFiles()
    {
        // Don't attempt to import/export files if the file picker is already open
        if (NativeFilePicker.IsFilePickerBusy())
        {
            return;
        }

        string[] extensions = new string[1];
        if (uiInputUTI.text.Length > 1)
        {
            extensions[0] = uiInputUTI.text;
        }
        else
        {
            extensions[0] = "public.item";
        }
        // Pick multiple files
        NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles((paths) =>
        {
            if (paths == null)
            {
                PrintDebug("Operation cancelled");
            }
            else
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    PrintDebug("Picked file: " + paths[i]);
                }
                    
            }
        }, extensions);

        PrintDebug("Permission result: " + permission);
    }

    private void TryIosExportASCII()
    {
        // Create a dummy text file
        string filePath = Path.Combine(Application.temporaryCachePath, "test.txt");
        File.WriteAllText(filePath, "Hello world!");

        // Export the file
        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePath, (success) => PrintDebug("File exported: " + success));

        PrintDebug("Permission result: " + permission);
    }

    private void TryIosExportBIN()
    {
        // Create a dummy binary file, stl file contains unusable data
        string filePath = Path.Combine(Application.temporaryCachePath, "test.stl");
        File.WriteAllBytes(filePath, new byte[] { 0, 1, 2, 3, 45, 200, 6 });

        // Export the file
        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePath, (success) => PrintDebug("File exported: " + success));

        PrintDebug("Permission result: " + permission);
    }

    private void PrintDebug(string msg)
    {
        uiDebugText.text += ("\n" + msg);
        Debug.Log(msg);
    }
}

﻿using HSDRaw.Common;
using HSDRawViewer.Converters;
using System;
using System.Windows.Forms;

namespace HSDRawViewer.ContextMenus
{
    public class JOBJContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(HSD_JOBJ) };

        public JOBJContextMenu() : base()
        {
            MenuItem Import = new MenuItem("Import Model From File");
            Import.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is HSD_JOBJ root)
                {
                    MainForm.SelectedDataNode.Collapse();
                    ModelImporter.ReplaceModelFromFile(root);
                }
            };
            MenuItems.Add(Import);

            MenuItem addChild = new MenuItem("Add Child");
            MenuItems.Add(addChild);

            MenuItem createJOBJ = new MenuItem("From Scratch");
            createJOBJ.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is HSD_JOBJ root)
                {
                    root.AddChild(new HSD_JOBJ()
                    {
                        SX = 1,
                        SY = 1,
                        SZ = 1,
                        Flags = JOBJ_FLAG.CLASSICAL_SCALING | JOBJ_FLAG.ROOT_XLU
                    });
                    MainForm.SelectedDataNode.Refresh();
                }
            };
            addChild.MenuItems.Add(createJOBJ);


            MenuItem createJOBJFromFile = new MenuItem("From File");
            createJOBJFromFile.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is HSD_JOBJ root)
                {
                    var f = Tools.FileIO.OpenFile(ApplicationSettings.HSDFileFilter);
                    if(f != null)
                    {
                        HSDRaw.HSDRawFile file = new HSDRaw.HSDRawFile(f);

                        var node = file.Roots[0].Data;
                        if (node is HSD_JOBJ newchild)
                            root.AddChild(newchild);
                    }
                    MainForm.SelectedDataNode.Refresh();
                }
            };
            addChild.MenuItems.Add(createJOBJFromFile);
        }
    }
}

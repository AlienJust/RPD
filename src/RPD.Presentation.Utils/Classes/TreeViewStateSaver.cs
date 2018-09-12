using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.IO;

namespace RPD.Presentation.Utils.Classes
{
    /// <summary>
    /// Сохраняет состояние TreeView. Утащено с инета.
    /// </summary>
    public class TreeViewState
    {
        readonly List<bool> _list = new List<bool>();
        private int _restoreTreeViewIndex = 0;
        readonly TreeView treeView;
        readonly string fileName;

        public TreeViewState(TreeView treeView, string fileName)
        {
            this.treeView = treeView;
            this.fileName = fileName;
        }

        public void Save()
        {
            _list.Clear();
            SaveTreeViewExpandedState(treeView.Items);

            XmlSerializer xs = new XmlSerializer(typeof(List<bool>));
            using (StreamWriter streamWriter = File.CreateText(fileName))
            {
                xs.Serialize(streamWriter, _list);
            }
        }

        public void Restore()
        {
            _list.Clear();
            _restoreTreeViewIndex = 0;
            RestoreTreeViewExpandedState(treeView.Items);
        }

        private void SaveTreeViewExpandedState(ItemCollection nodes)
        {
            foreach (object node in nodes)
            {
                if (node is TreeViewItem)
                {
                    TreeViewItem item = (TreeViewItem)node;
                    _list.Add(item.IsExpanded);
                    if (item.Items.Count > 0)
                    {
                        SaveTreeViewExpandedState(item.Items);
                    }
                }
            }
        }

        private void RestoreTreeViewExpandedState(ItemCollection nodes)
        {
            foreach (TreeViewItem node in nodes)
            {
                if (_restoreTreeViewIndex >= _list.Count)
                    break;

                node.IsExpanded = _list[_restoreTreeViewIndex++];

                if (node.Items.Count > 0)
                {
                    RestoreTreeViewExpandedState(node.Items);
                }
            }
        }
    }
}

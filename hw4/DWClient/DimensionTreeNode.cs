using System.Windows.Forms;
using DWClient.Models;

namespace DWClient
{
    public class DimensionTreeNode : TreeNode
    {
        public Dimension Dimension { get; set; }

        public DimensionTreeNode(Dimension dimension, string text) : base(text)
        {
            Dimension = dimension;
        }
    }
}

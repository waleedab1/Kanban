using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
        public readonly string emailCreator;
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator)
        {
            this.ColumnsNames = columnsNames;
            this.emailCreator = emailCreator;
        }

        internal Board(BusinessLayer.BoardPackage.Board b)
        {
            this.emailCreator = b.email;
            ColumnsNames = b.ColumnNames();
        }
    }
}

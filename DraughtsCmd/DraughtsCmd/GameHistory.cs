using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class GameHistory
    {
        public Stack<HistoryItem> Future { get; set; }
        public Stack<HistoryItem> Past { get; set; }
        public HistoryItem CurrItem { get; set; }

        public GameHistory()
        {
            Future = new Stack<HistoryItem>();
            Past = new Stack<HistoryItem>();
        }

        public void Undo()
        {
            if (Past.Count > 2)
            {
                Future.Push(Past.Pop());
                Future.Push(Past.Pop());

                CurrItem = Past.Peek();
            }
        }

        public void Redo()
        {
            if (Future.Count > 1)
            {
                Past.Push(Future.Pop());
                Past.Push(Future.Pop());

                CurrItem = Past.Peek();
            }
        }

        // Discard redo stack if item is added to move history
        public void AddItem(HistoryItem newItem)
        {
            Future = new Stack<HistoryItem>();

            Past.Push(newItem);
            CurrItem = Past.Peek();
        }
    }
}

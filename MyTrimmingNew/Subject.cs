using System.Collections.Generic;

namespace MyTrimmingNew
{
    public class Subject
    {
        private List<IVisualComponentObserver> Components { get; set; }

        public Subject()
        {
            Components = new List<IVisualComponentObserver>();
        }

        public void Attach(IVisualComponentObserver component)
        {
            Components.Add(component);
        }

        public void Detach(IVisualComponentObserver component)
        {
            Components.Remove(component);
        }

        public void Notify()
        {
            foreach(IVisualComponentObserver c in Components)
            {
                c.Update(this);
            }
        }
    }
}

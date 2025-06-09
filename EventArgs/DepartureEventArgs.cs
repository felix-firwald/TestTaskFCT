using TestTask.Models;

namespace TestTask.EventArgs
{
    public class DepartureEventArgs : System.EventArgs
    {
        public Truck Truck { get; }

        public DepartureEventArgs(Truck truck)
        {
            this.Truck = truck;
        }
    }
}

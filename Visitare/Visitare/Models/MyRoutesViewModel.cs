using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Visitare.Models
{
    public class MyRoutesViewModel
    {
        public List<Routes> RoutesList { get; set; }
        public Command<Routes> RemoveCommand
        {
            get
            {
                return new Command<Routes>((route) =>
                {
                    RoutesList.Remove(route);
                });
            }
        }

        public MyRoutesViewModel() { }
        
    }
}

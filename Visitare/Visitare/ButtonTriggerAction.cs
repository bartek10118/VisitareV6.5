using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Visitare
{
    public class ButtonTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            
                
                // should use TryParse or try/catch here
                int response = int.Parse(odpowie.Text);

                if (response > 0 & response < 4)
                {
                    // do something
                }
            

        }

    }
}

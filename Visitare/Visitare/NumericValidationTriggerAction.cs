using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Visitare
{
    public class NumericValidationTriggerAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry entry)
        {
            
                int result;

                bool isValid = Int32.TryParse(entry.Text, out result);

                if (result == 0 || result == 1 || result == 2 || result == 3)
                {
                    isValid = true;

                }
                else
                {
                    isValid = false;

                }
                entry.TextColor = isValid ? Color.Default : Color.Red;
            }
            
        }
    }


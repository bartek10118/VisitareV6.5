using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Visitare
{
    public class UserQuiz : INotifyPropertyChanged
    {
        private int points;

        public int Points
        {
            get
            {
                return points;
            }

            set
            {
                if (this.points != value)
                {
                    points = value;
                    OnPropertyChanged();
                }
            }
        }

        private string email;

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                if (this.email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string id;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                if (this.id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

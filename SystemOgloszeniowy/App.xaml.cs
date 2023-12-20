using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SystemOgloszeniowy.Entity;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        public static User? user = null;
        public static readonly string connectionString = "Data Source=(local)\\Local;Initial Catalog=Ogloszenia;Integrated Security=True";
        
    }
}

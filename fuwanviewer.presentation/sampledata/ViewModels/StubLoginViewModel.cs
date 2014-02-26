using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Presentation.ViewModels
{
    public class StubLoginViewModel : LoginViewModel
    {
        public StubLoginViewModel()
            : base(null)
        {
            base.Username = "Kreweta";
            base.Password = "kdfsjla";
            base.ErrorMessage = "Problem occured.";
        }
    }
}

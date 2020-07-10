using GPB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPB.ViewModels
{
    public class NavigationItem : ObservableObject
    {
        public NavigationItem(Type viewModel)
        {
            ViewModel = viewModel;
        }
        public NavigationItem(int glyph, string label, Type viewModel) : this(viewModel)
        {
            Label = label;
            Glyph = Char.ConvertFromUtf32(glyph).ToString();
        }

        public readonly string Glyph;
        public readonly string Label;
        public readonly Type ViewModel;

        private string _badge = null;
        public string Badge
        {
            get => _badge;
            set => Set(ref _badge, value);
        }
    }
}

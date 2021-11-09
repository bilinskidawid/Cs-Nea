using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_NEA_Phone
{
    public class TutorialPageFlyoutMenuItem
    {
        public TutorialPageFlyoutMenuItem()
        {
            TargetType = typeof(TutorialPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
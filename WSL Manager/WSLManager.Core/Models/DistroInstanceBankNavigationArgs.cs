using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace WSLManager.Core.Models
{
    public class DistroInstanceBankNavigationArgs:MvxViewModel
    {
        
        private Dictionary<string, DistroInstance> _bank;
        
        public DistroInstanceBankNavigationArgs(Dictionary<string, DistroInstance> _distroInstances)
        {
            if (_distroInstances == null)
            {
                DistroDictionaryBank = new Dictionary<string, DistroInstance>();
            }
            else
            {
                DistroDictionaryBank = _distroInstances;
            }
        }

        public Dictionary<string,DistroInstance> DistroDictionaryBank { get=>_bank; set=>SetProperty(ref _bank,value); }
    }
}
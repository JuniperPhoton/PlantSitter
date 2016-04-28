using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using PlantSitter.Common;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.ViewModel
{
    public class PlanDetailViewModel : ViewModelBase, INavigable
    {
        public bool IsFirstActived { get; set; } = true;

        public bool IsInView { get; set; }

        private UserPlanWrapped _currentPlanWrapped;
        public UserPlanWrapped CurrentPlanWrapped
        {
            get
            {
                return _currentPlanWrapped;
            }
            set
            {
                if (_currentPlanWrapped != value)
                {
                    _currentPlanWrapped = value;
                    RaisePropertyChanged(() => CurrentPlanWrapped);
                }
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand != null) return _deleteCommand;
                return _deleteCommand = new RelayCommand(async() =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确实要删除吗？删除后无法恢复");
                      ds.OnLeftBtnClick += async (s) =>
                      {
                          NavigationService.RootFrame.GoBack();
                          App.VMLocator.UsersPlansVM.DeletePlan(this.CurrentPlanWrapped);
                          var result = await CloudService.DeletePlan(CurrentPlanWrapped.CurrentPlan.Gid, CTSFactory.MakeCTS().Token);
                      };
                      await ds.ShowAsync();
                  });
            }
        }

        private RelayCommand _setMainCommand;
        public RelayCommand SetMainCommand
        {
            get
            {
                if (_setMainCommand != null) return _setMainCommand;
                return _setMainCommand = new RelayCommand(async () =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确实要设置为照看中吗？");
                      ds.OnLeftBtnClick += async (s) =>
                      {
                          App.VMLocator.UsersPlansVM.SetMain(this.CurrentPlanWrapped);
                          var result = await CloudService.SetMainPlan(CurrentPlanWrapped.CurrentPlan.Gid, CTSFactory.MakeCTS().Token);
                          ToastService.SendToast("设置成功 :-P");
                      };
                      await ds.ShowAsync();
                  });
            }
        }


        public PlanDetailViewModel()
        {

        }

        public void Activate(object param)
        {
            this.CurrentPlanWrapped = param as UserPlanWrapped;
        }

        public void Deactivate(object param)
        {

        }

        public void OnLoaded()
        {

        }
    }
}

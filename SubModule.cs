using GearLossOnCapture.Behaviors;
using GearLossOnCapture.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace GearLossOnCapture
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);
            if (gameStarter is CampaignGameStarter campaignGameStarter)
            {
                ModSettings.Reload();
                campaignGameStarter.AddBehavior(new GearLossBehavior());
            }
        }
    }
}

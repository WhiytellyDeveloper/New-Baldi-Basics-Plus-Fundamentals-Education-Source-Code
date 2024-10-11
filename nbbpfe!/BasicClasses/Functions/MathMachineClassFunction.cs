using nbbpfe.Enums;
using nbppfe.CustomContent.CustomItems;
using nbppfe.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace nbppfe.BasicClasses.Functions
{
    public class MathMachineClassFunction : RoomFunction
    {
        public override void OnPlayerEnter(PlayerManager player)
        {
            base.OnPlayerEnter(player);

            if (player.itm.Has(CustomItemsEnum.HomeworkTierA.ToItemEnum()))
            {
                foreach (Activity activity in player.ec.activities)
                {
                    if (player.ec.CellFromPosition(activity.transform.position).room == room && !activity.IsCompleted)
                    {
                        Instantiate<Item>(CustomItemsEnum.HomeworkTierA.ToItem().item).GetComponent<ITM_Homework>().OnUse();
                        activity.Completed(player.playerNumber, true, activity);
                        foreach (Notebook notebook in player.ec.notebooks)
                        {
                            if (player.ec.CellFromPosition(notebook.transform.position).room == room)
                                notebook.Clicked(player.playerNumber);
                        }
                            player.itm.Remove(CustomItemsEnum.HomeworkTierA.ToItemEnum());
                    }
                }
            }
        }
    }
}

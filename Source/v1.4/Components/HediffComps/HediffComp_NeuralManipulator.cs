using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ATReforged
{
    // This hediff comp is responsible for showing the various Gizmo's for controlling pawns. No controls are visible if a SkyMind connection or SkyMind Core is missing.
    public class HediffComp_NeuralManipulator : HediffComp
    {
        public ManipulationProtocol Protocol
        {
            get
            {
                return protocol;
            }
            set
            {
                ManipulationProtocol oldProtocol = protocol;
                protocol = value;
                
                if (oldProtocol == protocol)
                {
                    return;
                }

                // The old protocol hediff should be removed in preparation for any new hediff. Removal of a hediff may have side-effects handled automatically.
                if (protocolHediff != null)
                {
                    Pawn.health.RemoveHediff(protocolHediff);
                    protocolHediff = null;
                }
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();

            protocol = ManipulationProtocol.None;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            Scribe_Values.Look(ref protocol, "ATNM_manipulationProtocol", ManipulationProtocol.None);
            Scribe_Values.Look(ref lastCommandTick, "ATNM_lastCommandTick", -1250);
            Scribe_References.Look(ref protocolHediff, "ATNM_protocolHediff");
        }

        // If the hediff this is attached to is removed, do cleanup.
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            Protocol = ManipulationProtocol.None;
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (!Utils.IsValidMindTransferTarget(Pawn) || Utils.gameComp.GetSkyMindCloudCapacity() == 0)
                yield break;

            // The gizmo will open a float menu with options when clicked (if it isn't disabled).
            Command_Action gizmo = new Command_Action
            {
                icon = Tex.ManipulationIcon,
                defaultLabel = "ATNM_SetProtocol".Translate(),
                defaultDesc = "ATNM_SetProtocolDesc".Translate(protocol.ToString()),
                action = delegate ()
                {
                    List<FloatMenuOption> opts = new List<FloatMenuOption>();

                    // Harvest and Replacement protocols can not be cancelled or be switched away from.
                    if (protocol == ManipulationProtocol.Harvest || protocol == ManipulationProtocol.Replacement)
                    {
                        opts.Add(new FloatMenuOption("ATNM_CanNotCancelProtocol".Translate(), null));
                        Find.WindowStack.Add(new FloatMenu(opts, "ATNM_SetProtocol".Translate()));
                        return;
                    }

                    // Always allow and put None at the top. There are no extra conditions.
                    opts.Add(new FloatMenuOption("ATNM_NoneProtocol".Translate(), delegate
                    {
                        Protocol = ManipulationProtocol.None;
                    })
                    {
                        tooltip = new TipSignal("ATNM_NoneProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Rampage protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("ATNM_RampageProtocol".Translate(), delegate
                    {
                        Protocol = ManipulationProtocol.Rampage;
                        protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_RampageProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("ATNM_RampageProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Stasis protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("ATNM_StasisProtocol".Translate(), delegate
                    {
                        Protocol = ManipulationProtocol.Stasis;
                        protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_StasisProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("ATNM_StasisProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Terror protocol. Only available for slave pawns (which requires Ideology DLC).
                    if (Pawn.IsSlaveOfColony)
                    {
                        opts.Add(new FloatMenuOption("ATNM_TerrorProtocol".Translate(), delegate
                        {
                            Protocol = ManipulationProtocol.Terror;
                            protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_TerrorProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("ATNM_TerrorProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("ATNM_TerrorProtocolRequiresSlave".Translate(), null));
                    }

                    // Allow Numbness protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("ATNM_NumbnessProtocol".Translate(), delegate
                    {
                        Protocol = ManipulationProtocol.Numbness;
                        protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_NumbnessProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("ATNM_NumbnessProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Harvest protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("ATNM_HarvestProtocol".Translate(), delegate
                    {
                        Find.WindowStack.Add(new Dialog_MessageBox("ATR_AbsorbExperienceConfirm".Translate(Pawn.LabelShortCap) + "\n" + "ATR_SkyMindDisconnectionRisk".Translate(), "Confirm".Translate(), buttonBText: "Cancel".Translate(), title: "ATR_AbsorbExperience".Translate(), buttonAAction: delegate
                        {
                            Protocol = ManipulationProtocol.Harvest;
                            Pawn.GetComp<CompSkyMindLink>().InitiateConnection(3);
                            lastCommandTick = Find.TickManager.TicksGame;
                        }));
                    })
                    {
                        tooltip = new TipSignal("ATNM_HarvestProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Cultivation protocol. Can only be used on non-prisoners.
                    if (Pawn.IsFreeColonist)
                    {
                        opts.Add(new FloatMenuOption("ATNM_CultivationProtocol".Translate(), delegate
                        {
                            Protocol = ManipulationProtocol.Cultivation;
                            protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_CultivationProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("ATNM_CultivationProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("ATNM_CultivationProtocolRequiresNonPrisoner".Translate(), null));
                    }

                    // Allow Conversion protocol. Only available for pawns that do not have the "Unwavering" condition.
                    if (Pawn.Faction == Faction.OfPlayer || Pawn.guest.Recruitable)
                    {
                        opts.Add(new FloatMenuOption("ATNM_ConversionProtocol".Translate(), delegate
                        {
                            Protocol = ManipulationProtocol.Conversion;
                            protocolHediff = HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_ConversionProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("ATNM_ConversionProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("ATNM_ConversionProtocolUnusableOnUnwavering".Translate(), null));
                    }

                    // Allow Replacement protocol. There are no extra conditions. This protocol destroys the chip upon activation, and has a warning option attached.
                    opts.Add(new FloatMenuOption("ATNM_ReplacementProtocol".Translate(), delegate
                    {
                        Find.WindowStack.Add(new Dialog_MessageBox("ATNM_ReplacementProtocolConfirmDesc".Translate(Pawn.LabelShortCap), "Confirm".Translate(), buttonBText: "Cancel".Translate(), title: "ATNM_ReplacementProtocol".Translate(), buttonAAction: delegate
                        {
                            Protocol = ManipulationProtocol.Replacement;
                            // Don't give the replacement hediff to protocolHediff or it'll be removed when this comp deletes the parent hediff.
                            Pawn.health.AddHediff(HediffMaker.MakeHediff(ATNM_HediffDefOf.ATNM_ReplacementProtocol, Pawn));
                            Pawn.health.RemoveHediff(parent);
                        }));
                    })
                    {
                        tooltip = new TipSignal("ATNM_ReplacementProtocolTooltip".Translate(), 0.1f)
                    });

                    Find.WindowStack.Add(new FloatMenu(opts, "ATNM_SetProtocol".Translate()));
                }
            };
            // New protocols can only be issued once every half hour.
            if (Find.TickManager.TicksGame - 1250 < lastCommandTick)
            {
                gizmo.disabled = true;
                gizmo.disabledReason = "ATNM_protocolChangedTooRecently".Translate();
            }
            yield return gizmo;
        }

        Hediff protocolHediff;
        ManipulationProtocol protocol;
        int lastCommandTick = -1250;
    }
}

using RimWorld;
using System.Collections.Generic;
using Verse;

namespace SkyMind
{
    // This hediff comp is responsible for showing the various Gizmo's for controlling pawns. No controls are visible if a SkyMind connection or SkyMind Core is missing.
    public class HediffComp_NeuralManipulator : HediffComp
    {
        public SMNM_ManipulationProtocol Protocol
        {
            get
            {
                return protocol;
            }
            set
            {
                SMNM_ManipulationProtocol oldProtocol = protocol;
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

            protocol = SMNM_ManipulationProtocol.None;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            Scribe_Values.Look(ref protocol, "SMNM_SMNM_ManipulationProtocol", SMNM_ManipulationProtocol.None);
            Scribe_Values.Look(ref lastCommandTick, "SMNM_lastCommandTick", -1250);
            Scribe_References.Look(ref protocolHediff, "SMNM_protocolHediff");
        }

        // If the hediff this is attached to is removed, do cleanup.
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            Protocol = SMNM_ManipulationProtocol.None;
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (!SMN_Utils.IsValidMindTransferTarget(Pawn) || !SMN_Utils.gameComp.HasSkyMindCore())
                yield break;

            // The gizmo will open a float menu with options when clicked (if it isn't disabled).
            Command_Action gizmo = new Command_Action
            {
                icon = SMNM_Tex.ManipulationIcon,
                defaultLabel = "SMNM_SetProtocol".Translate(),
                defaultDesc = "SMNM_SetProtocolDesc".Translate(protocol.ToString()),
                action = delegate ()
                {
                    List<FloatMenuOption> opts = new List<FloatMenuOption>();

                    // Harvest and Replacement protocols can not be cancelled or be switched away from.
                    if (protocol == SMNM_ManipulationProtocol.Harvest || protocol == SMNM_ManipulationProtocol.Replacement)
                    {
                        opts.Add(new FloatMenuOption("SMNM_CanNotCancelProtocol".Translate(), null));
                        Find.WindowStack.Add(new FloatMenu(opts, "SMNM_SetProtocol".Translate()));
                        return;
                    }

                    // Always allow and put None at the top. There are no extra conditions.
                    opts.Add(new FloatMenuOption("SMNM_NoneProtocol".Translate(), delegate
                    {
                        Protocol = SMNM_ManipulationProtocol.None;
                    })
                    {
                        tooltip = new TipSignal("SMNM_NoneProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Rampage protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("SMNM_RampageProtocol".Translate(), delegate
                    {
                        Protocol = SMNM_ManipulationProtocol.Rampage;
                        protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_RampageProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("SMNM_RampageProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Stasis protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("SMNM_StasisProtocol".Translate(), delegate
                    {
                        Protocol = SMNM_ManipulationProtocol.Stasis;
                        protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_StasisProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("SMNM_StasisProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Terror protocol. Only available for slave pawns (which requires Ideology DLC).
                    if (Pawn.IsSlaveOfColony)
                    {
                        opts.Add(new FloatMenuOption("SMNM_TerrorProtocol".Translate(), delegate
                        {
                            Protocol = SMNM_ManipulationProtocol.Terror;
                            protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_TerrorProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("SMNM_TerrorProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("SMNM_TerrorProtocolRequiresSlave".Translate(), null));
                    }

                    // Allow Numbness protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("SMNM_NumbnessProtocol".Translate(), delegate
                    {
                        Protocol = SMNM_ManipulationProtocol.Numbness;
                        protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_NumbnessProtocol, Pawn);
                        Pawn.health.AddHediff(protocolHediff);
                        lastCommandTick = Find.TickManager.TicksGame;
                    })
                    {
                        tooltip = new TipSignal("SMNM_NumbnessProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Harvest protocol. There are no extra conditions.
                    opts.Add(new FloatMenuOption("SMNM_HarvestProtocol".Translate(), delegate
                    {
                        Find.WindowStack.Add(new Dialog_MessageBox("SMNM_AbsorbExperienceConfirm".Translate(Pawn.LabelShortCap) + "\n" + "SMNM_SkyMindDisconnectionRisk".Translate(), "Confirm".Translate(), buttonBText: "Cancel".Translate(), title: "SMNM_AbsorbExperience".Translate(), buttonAAction: delegate
                        {
                            Protocol = SMNM_ManipulationProtocol.Harvest;
                            Pawn.GetComp<CompSkyMindLink>().InitiateConnection(3);
                            lastCommandTick = Find.TickManager.TicksGame;
                        }));
                    })
                    {
                        tooltip = new TipSignal("SMNM_HarvestProtocolTooltip".Translate(), 0.1f)
                    });

                    // Allow Cultivation protocol. Can only be used on non-prisoners.
                    if (Pawn.IsFreeColonist)
                    {
                        opts.Add(new FloatMenuOption("SMNM_CultivationProtocol".Translate(), delegate
                        {
                            Protocol = SMNM_ManipulationProtocol.Cultivation;
                            protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_CultivationProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("SMNM_CultivationProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("SMNM_CultivationProtocolRequiresNonPrisoner".Translate(), null));
                    }

                    // Allow Conversion protocol. Only available for pawns that do not have the "Unwavering" condition.
                    if (Pawn.Faction == Faction.OfPlayer || Pawn.guest.Recruitable)
                    {
                        opts.Add(new FloatMenuOption("SMNM_ConversionProtocol".Translate(), delegate
                        {
                            Protocol = SMNM_ManipulationProtocol.Conversion;
                            protocolHediff = HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_ConversionProtocol, Pawn);
                            Pawn.health.AddHediff(protocolHediff);
                            lastCommandTick = Find.TickManager.TicksGame;
                        })
                        {
                            tooltip = new TipSignal("SMNM_ConversionProtocolTooltip".Translate(), 0.1f)
                        });
                    }
                    else
                    {
                        opts.Add(new FloatMenuOption("SMNM_ConversionProtocolUnusableOnUnwavering".Translate(), null));
                    }

                    // Allow Replacement protocol. There are no extra conditions. This protocol destroys the chip upon activation, and has a warning option attached.
                    opts.Add(new FloatMenuOption("SMNM_ReplacementProtocol".Translate(), delegate
                    {
                        Find.WindowStack.Add(new Dialog_MessageBox("SMNM_ReplacementProtocolConfirmDesc".Translate(Pawn.LabelShortCap), "Confirm".Translate(), buttonBText: "Cancel".Translate(), title: "SMNM_ReplacementProtocol".Translate(), buttonAAction: delegate
                        {
                            Protocol = SMNM_ManipulationProtocol.Replacement;
                            // Don't give the replacement hediff to protocolHediff or it'll be removed when this comp deletes the parent hediff.
                            Pawn.health.AddHediff(HediffMaker.MakeHediff(SMNM_HediffDefOf.SMNM_ReplacementProtocol, Pawn));
                            Pawn.health.RemoveHediff(parent);
                        }));
                    })
                    {
                        tooltip = new TipSignal("SMNM_ReplacementProtocolTooltip".Translate(), 0.1f)
                    });

                    Find.WindowStack.Add(new FloatMenu(opts, "SMNM_SetProtocol".Translate()));
                }
            };
            // New protocols can only be issued once every half hour.
            if (Find.TickManager.TicksGame - 1250 < lastCommandTick)
            {
                gizmo.disabled = true;
                gizmo.disabledReason = "SMNM_protocolChangedTooRecently".Translate();
            }
            yield return gizmo;
        }

        Hediff protocolHediff;
        SMNM_ManipulationProtocol protocol;
        int lastCommandTick = -1250;
    }
}

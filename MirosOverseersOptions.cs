using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MirosOverseers;

public class MirosOverseersOptions : OptionInterface
{
    public readonly MirosOverseers modInstance;

    public readonly Configurable<bool> GuaranteeIggy;
    public readonly Configurable<bool> GuaranteeWildOverseers;
    public readonly Configurable<bool> AllowEarlyOverseers;
    public readonly Configurable<bool> AdjustOverseerSpawns;
    public readonly Configurable<float> OverseersMinPlus;
    public readonly Configurable<float> OverseersMinTimes;
    public readonly Configurable<float> OverseersMaxPlus;
    public readonly Configurable<float> OverseersMaxTimes;
    public readonly Configurable<float> OverseerReaimCooldown;
    public readonly Configurable<float> OverseerFiringCooldown;
    public readonly Configurable<float> OverseerAimingDuration;
    public readonly Configurable<bool> ArtificerVulnerability;
    public readonly Configurable<bool> OverseersOverseerImmune;
    public readonly Configurable<bool> OverseersSpearImmune;
    public readonly Configurable<bool> OverseersExplosionImmune;
    public readonly Configurable<bool> OverseersImmortal;
    public readonly Configurable<bool> DisableLaserShader;
    public readonly Configurable<bool> ColorChangingLaser;
    public readonly Configurable<bool> LightweightExplosions;
    public readonly Configurable<bool> DisableTinnitus;
    public readonly Configurable<bool> DisableDuringCutscenes;
    public readonly Configurable<bool> DisableDuringForcedInput;
    public readonly Configurable<bool> DisableNearPuppets;
    public readonly Configurable<bool> DisableDuringDialogue;

    public OpCheckBox GuaranteeIggyCheckbox;
    public OpCheckBox GuaranteeWildOverseersCheckbox;
    public OpCheckBox AllowEarlyOverseersCheckbox;
    public OpCheckBox AdjustOverseerSpawnsCheckbox;
    public OpTextBox OverseersMinPlusTextbox;
    public OpTextBox OverseersMinTimesTextbox;
    public OpTextBox OverseersMaxPlusTextbox;
    public OpTextBox OverseersMaxTimesTextbox;
    public OpTextBox OverseerReaimCooldownTextbox;
    public OpTextBox OverseerFiringCooldownTextbox;
    public OpTextBox OverseerAimingDurationTextbox;
    public OpCheckBox ArtificerVulnerabilityCheckbox;
    public OpCheckBox OverseersOverseerImmuneCheckbox;
    public OpCheckBox OverseersSpearImmuneCheckbox;
    public OpCheckBox OverseersExplosionImmuneCheckbox;
    public OpCheckBox OverseersImmortalCheckbox;
    public OpCheckBox DisableLaserShaderCheckbox;
    public OpCheckBox ColorChangingLaserCheckbox;
    public OpCheckBox LightweightExplosionsCheckbox;
    public OpCheckBox DisableTinnitusCheckbox;
    public OpCheckBox DisableDuringCutscenesCheckbox;
    public OpCheckBox DisableDuringForcedInputCheckbox;
    public OpCheckBox DisableNearPuppetsCheckbox;
    public OpCheckBox DisableDuringDialogueCheckbox;

    public OpHoldButton RelaxedButton;
    public OpHoldButton ChallengingButton;
    public OpHoldButton HellishButton;

    public MirosOverseersOptions(MirosOverseers modInstance)
    {
        this.modInstance = modInstance;

        GuaranteeIggy = config.Bind("GuaranteeIggy", true);
        GuaranteeWildOverseers = config.Bind("GuaranteeWildOverseers", true);
        AllowEarlyOverseers = config.Bind("AllowEarlyOverseers", true);
        AdjustOverseerSpawns = config.Bind("AdjustOverseerSpawns", true);
        OverseersMinPlus = config.Bind("OverseersMinPlus", 5f);
        OverseersMinTimes = config.Bind("OverseersMinTimes", 2f);
        OverseersMaxPlus = config.Bind("OverseersMaxPlus", 5f);
        OverseersMaxTimes = config.Bind("OverseersMaxTimes", 2f);
        OverseerReaimCooldown = config.Bind("OverseerReaimCooldown", 0.5f);
        OverseerFiringCooldown = config.Bind("OverseerFiringCooldown", 2f);
        OverseerAimingDuration = config.Bind("OverseerAimingDuration", 5f);
        ArtificerVulnerability = config.Bind("ArtificerVulnerability", true);
        OverseersOverseerImmune = config.Bind("OverseersOverseerImmune", false);
        OverseersSpearImmune = config.Bind("OverseersSpearImmune", false);
        OverseersExplosionImmune = config.Bind("OverseersExplosionImmune", false);
        OverseersImmortal = config.Bind("OverseersImmortal", false);
        DisableLaserShader = config.Bind("DisableLaserShader", false);
        ColorChangingLaser = config.Bind("ColorChangingLaser", false);
        LightweightExplosions = config.Bind("LightweightExplosions", false);
        DisableTinnitus = config.Bind("DisableTinnitus", false);
        DisableDuringCutscenes = config.Bind("DisableDuringCutscenes", true);
        DisableDuringForcedInput = config.Bind("DisableDuringForcedInput", true);
        DisableNearPuppets = config.Bind("DisableNearPuppets", false);
        DisableDuringDialogue = config.Bind("DisableDuringDialogue", false);
    }
    public void SetRelaxed(UIfocusable trigger)
    {
        GuaranteeIggyCheckbox.value = "false"; //The fact that this is stored as a string is so stupid, I'm using it instead of SetValueBool() out of spite.
        GuaranteeWildOverseersCheckbox.value = "false";
        AllowEarlyOverseersCheckbox.value = "true";
        AdjustOverseerSpawnsCheckbox.value = "false";
        //Don't need to adjust the spawn multipliers if we're disabling them altogether
        OverseerReaimCooldownTextbox.value = "1";
        OverseerFiringCooldownTextbox.value = "3";
        OverseerAimingDurationTextbox.value = "5";
        ArtificerVulnerabilityCheckbox.value = "false";
        OverseersOverseerImmuneCheckbox.value = "false";
        OverseersSpearImmuneCheckbox.value = "false";
        OverseersExplosionImmuneCheckbox.value = "false";
        OverseersImmortalCheckbox.value = "false";
        //Don't adjust visual stuff
        DisableDuringCutscenesCheckbox.value = "true";
        DisableDuringForcedInputCheckbox.value = "true";
        DisableNearPuppetsCheckbox.value = "true";
        DisableDuringDialogueCheckbox.value = "true";
    }
    public void SetChallenging(UIfocusable trigger)
    {
        GuaranteeIggyCheckbox.value = "true";
        GuaranteeWildOverseersCheckbox.value = "true";
        //Don't need to add cycle 0 spawns when it's already forced
        AdjustOverseerSpawnsCheckbox.value = "true";
        OverseersMinPlusTextbox.value = "5";
        OverseersMinTimesTextbox.value = "2";
        OverseersMaxPlusTextbox.value = "5";
        OverseersMaxTimesTextbox.value = "2";
        OverseerReaimCooldownTextbox.value = "0.5";
        OverseerFiringCooldownTextbox.value = "2";
        OverseerAimingDurationTextbox.value = "5";
        ArtificerVulnerabilityCheckbox.value = "true";
        OverseersOverseerImmuneCheckbox.value = "false";
        OverseersSpearImmuneCheckbox.value = "false";
        OverseersExplosionImmuneCheckbox.value = "false";
        OverseersImmortalCheckbox.value = "false";
        //Don't adjust visual stuff
        DisableDuringCutscenesCheckbox.value = "true";
        DisableDuringForcedInputCheckbox.value = "true";
        DisableNearPuppetsCheckbox.value = "false";
        DisableDuringDialogueCheckbox.value = "false";
    }
    public void SetHellish(UIfocusable trigger)
    {
        GuaranteeIggyCheckbox.value = "true";
        GuaranteeWildOverseersCheckbox.value = "true";
        //Don't need to add cycle 0 spawns when it's already forced
        AdjustOverseerSpawnsCheckbox.value = "true";
        OverseersMinPlusTextbox.value = "10";
        OverseersMinTimesTextbox.value = "3";
        OverseersMaxPlusTextbox.value = "10";
        OverseersMaxTimesTextbox.value = "3";
        OverseerReaimCooldownTextbox.value = "0.25";
        OverseerFiringCooldownTextbox.value = "0";
        OverseerAimingDurationTextbox.value = "3";
        ArtificerVulnerabilityCheckbox.value = "true";
        OverseersOverseerImmuneCheckbox.value = "true";
        OverseersSpearImmuneCheckbox.value = "false";
        OverseersExplosionImmuneCheckbox.value = "false";
        OverseersImmortalCheckbox.value = "false";
        //Don't adjust visual stuff
        DisableDuringCutscenesCheckbox.value = "false";
        DisableDuringForcedInputCheckbox.value = "false";
        DisableNearPuppetsCheckbox.value = "false";
        DisableDuringDialogueCheckbox.value = "false";
    }
    public void ToggleSpawnrateTextboxes()
    {
        OverseersMinPlusTextbox.greyedOut = !AdjustOverseerSpawnsCheckbox.GetValueBool();
        OverseersMinTimesTextbox.greyedOut = !AdjustOverseerSpawnsCheckbox.GetValueBool();
        OverseersMaxPlusTextbox.greyedOut = !AdjustOverseerSpawnsCheckbox.GetValueBool();
        OverseersMaxTimesTextbox.greyedOut = !AdjustOverseerSpawnsCheckbox.GetValueBool();
    }
    public void UpdateAllowEarlyOverseers()
    {
        //if (GuaranteeWildOverseersCheckbox.GetValueBool()) { AllowEarlyOverseersCheckbox.value = "true"; }
        AllowEarlyOverseersCheckbox.greyedOut = GuaranteeWildOverseersCheckbox.GetValueBool();
    }
    public void UpdateDisableDuringCutscenes()
    {
        DisableDuringCutscenesCheckbox.greyedOut = !ModManager.JollyCoop;
    }

    public override void Initialize()
    {
        var opTab = new OpTab(this, "Options");
        Tabs = [opTab];
        List<List<UIelement>> UnbuiltPlayerOptions =
        [
            [new OpLabel(0f, 0f, "Presets:")],
            [
                RelaxedButton = new OpHoldButton(new Vector2(0f, 0f), new Vector2(0f, 0f), "Relaxed") { description = "Leaves overseer spawnrates default, and enables some safeties; designed for newer players, or for use with other mods." },
                ChallengingButton = new OpHoldButton(new Vector2(0f, 0f), new Vector2(0f, 0f), "Challenging") { description = "The intended (and default) experience; increases overseer spawnrates to give experienced players an interesting but mostly™ fair challenge." },
                HellishButton = new OpHoldButton(new Vector2(0f, 0f), new Vector2(0f, 0f), "Hellish") { description = "This *will* be painful. Overseers aim dramatically faster, can't kill each other, and there are far more of them. Designed for seasoned movement nerds looking for a serious challenge." },
            ],

            [new OpLabel(0f, 0f, "Overseer spawning behavior:")],
            [
                GuaranteeIggyCheckbox = new OpCheckBox(GuaranteeIggy, 0f, 0f) { description = "Skips the %-chance for iggy to spawn per cycle, normally based on the current region." },
                new OpLabel(0f, 0f, "Guarantee Iggy tries to spawn?") { description = "Skips the %-chance for iggy to spawn per cycle, normally based on the current region." },
            ],
            [
                GuaranteeWildOverseersCheckbox = new OpCheckBox(GuaranteeWildOverseers, 0f, 0f) { description = "Skips the %-chance for \"wild\" overseers to spawn per cycle, normally based on the current region and cycle count." },
                new OpLabel(0f, 0f, "Guarantee \"wild\" overseers try to spawn?") { description = "Skips the %-chance for \"wild\" overseers to spawn per cycle, normally based on the current region and cycle count." },
            ],
            [
                AllowEarlyOverseersCheckbox = new OpCheckBox(AllowEarlyOverseers, 0f, 0f) { description = "\"Wild\" overseers spawn more frequently the higher the cycle count is; normally cycles 0-2's chance is multiplied by 0%." },
                new OpLabel(0f, 0f, "Allow overseers on cycles 0-2?") { description = "\"Wild\" overseers spawn more frequently the higher the cycle count is; normally cycles 0-2's chance is multiplied by 0%." },
            ],
            [
                AdjustOverseerSpawnsCheckbox = new OpCheckBox(AdjustOverseerSpawns, 0f, 0f) { description = "Toggles whether or not the following modifiers should be applied or not." },
                new OpLabel(0f, 0f, "Modify overseer spawnrates?") { description = "Toggles whether or not the following modifiers should be applied or not." },
            ],
            [
                OverseersMinPlusTextbox = new OpTextBox(OverseersMinPlus, new Vector2(0f, 0f), 0f) { description = "Adjusts the minimum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
                OverseersMinTimesTextbox = new OpTextBox(OverseersMinTimes, new Vector2(0f, 0f), 0f) { description = "Adjusts the minimum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
                new OpLabel(0f, 0f, "Minimum overseers: +X, then *Y") { description = "Adjusts the minimum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
            ],
            [
                OverseersMaxPlusTextbox = new OpTextBox(OverseersMaxPlus, new Vector2(0f, 0f), 0f) { description = "Adjusts the maximum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
                OverseersMaxTimesTextbox = new OpTextBox(OverseersMaxTimes, new Vector2(0f, 0f), 0f) { description = "Adjusts the minimum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
                new OpLabel(0f, 0f, "Maximum overseers: +X, then *Y") { description = "Adjusts the minimum number of \"wild\" overseers that can spawn per cycle. The first value adds to the current region, then the second value multiplies it." },
            ],

            [new OpLabel(0f, 0f, "Laser statistics (in seconds):")],
            [
                OverseerReaimCooldownTextbox = new OpTextBox(OverseerReaimCooldown, new Vector2(0f, 0f), 0f) { description = "Adjusts how long overseers are blocked from aiming after emerging from a zip. Both cooldowns run in parallel." },
                new OpLabel(0f, 0f, "Cooldown after teleporting") { description = "Adjusts how long overseers are blocked from aiming after emerging from a zip. Both cooldowns run in parallel." },
            ],
            [
                OverseerFiringCooldownTextbox = new OpTextBox(OverseerFiringCooldown, new Vector2(0f, 0f), 0f) { description = "Adjusts how long overseers are blocked from aiming after successfully firing. Only ticks when emerged. Both cooldowns run in parallel." },
                new OpLabel(0f, 0f, "Cooldown after firing") { description = "Adjusts how long overseers are blocked from aiming after successfully firing. Only ticks when emerged. Both cooldowns run in parallel." },
            ],
            [
                OverseerAimingDurationTextbox = new OpTextBox(OverseerAimingDuration, new Vector2(0f, 0f), 0f) { description = "Controls how long the laser takes to fire (Miros Vultures take 4.75 seconds). Allow me to read your mind; yes this supports 0, and no that's not a good idea." },
                new OpLabel(0f, 0f, "Aiming speed") { description = "Controls how long the laser takes to fire (Miros Vultures take 4.75 seconds). Allow me to read your mind; yes this supports 0, and no that's not a good idea." },
            ],
            [
                ArtificerVulnerabilityCheckbox = new OpCheckBox(ArtificerVulnerability, 0f, 0f) { description = "Allows overseer laser explosions to kill Artificer. Artificer will remain immune (or technically 5x resistant) to other types of explosions." },
                new OpLabel(0f, 0f, "Overseer lasers can kill Artificer?") { description = "Allows overseer laser explosions to kill Artificer. Artificer will remain immune (or technically 5x resistant) to other types of explosions." },
            ],

            [new OpLabel(0f, 0f, "Overseer immunities:")],
            [
                OverseersOverseerImmuneCheckbox = new OpCheckBox(OverseersOverseerImmune, 0f, 0f) { description = "Prevents overseers' explosions from damaging other overseers. Overseers are automatically immune to their own explosions." },
                new OpLabel(0f, 0f, "Are overseers immune to each other?") { description = "Prevents overseers' explosions from damaging other overseers. Overseers are automatically immune to their own explosions." },
            ],
            [
                OverseersSpearImmuneCheckbox = new OpCheckBox(OverseersSpearImmune, 0f, 0f) { description = "Makes all overseers immune to rocks, spears, and any other similar thrown weapons. Spearmaster's red Iggy (Riggy?) is immune to spears by default." },
                new OpLabel(0f, 0f, "Are overseers immune to spears?") { description = "Makes all overseers immune to rocks, spears, and any other similar thrown weapons. Spearmaster's red Iggy (Riggy?) is immune to spears by default." },
            ],
            [
                OverseersExplosionImmuneCheckbox = new OpCheckBox(OverseersExplosionImmune, 0f, 0f) { description = "Makes all overseers immune to all explosion damage. Overseers are automatically immune to their own explosions, but not other overseers' explosions." },
                new OpLabel(0f, 0f, "Are overseers immune to explosions?") { description = "Makes overseers immune to all explosion damage. Overseers are automatically immune to their own explosions, but not other overseers' explosions." },
            ],
            [
                OverseersImmortalCheckbox = new OpCheckBox(OverseersImmortal, 0f, 0f) { description = "Makes all overseers completely immortal, for all you masochists out there." },
                new OpLabel(0f, 0f, "Are overseers immortal?") { description = "Makes all overseers completely immortal, for all you masochists out there." },
            ],

            [new OpLabel(0f, 0f, "VFX/SFX tweaks:")],
            [
                DisableLaserShaderCheckbox = new OpCheckBox(DisableLaserShader, 0f, 0f) { description = "Makes the laser sight solid instead of \"glittery\". Miros laser effects are designed for dark environments and may be hard to see elsewhere." },
                new OpLabel(0f, 0f, "Disable the laser sight's \"hologram\" effect?") { description = "Makes the laser sight solid instead of \"glittery\". Miros laser effects are designed for dark environments and may be hard to see elsewhere." },
            ],
            [
                ColorChangingLaserCheckbox = new OpCheckBox(ColorChangingLaser, 0f, 0f) { description = "Makes the laser sight change color alongside the target glow. Miros laser effects are designed for dark environments and may be hard to see elsewhere." },
                new OpLabel(0f, 0f, "Change the laser sight's color based on charge status?") { description = "Makes the laser sight change color alongside the target glow. Miros laser effects are designed for dark environments and may be hard to see elsewhere." },
            ],
            [
                LightweightExplosionsCheckbox = new OpCheckBox(LightweightExplosions, 0f, 0f) { description = "Removes certain visual effects from overseer laser explosions that contribute greatly to their lag at large scale." },
                new OpLabel(0f, 0f, "Lightweight laser explosion VFX?") { description = "Removes certain visual effects from overseer laser explosions that contribute greatly to their lag at large scale." },
            ],
            [
                DisableTinnitusCheckbox = new OpCheckBox(DisableTinnitus, 0f, 0f) { description = "Overseer laser explosions no longer give you tinnitus, or deafen you whatsoever. Other explosions will still deafen you as normal." },
                new OpLabel(0f, 0f, "Disable laser explosion tinnitus?") { description = "Overseer laser explosions no longer give you tinnitus, or deafen you whatsoever. Other explosions will still deafen you as normal." },
            ],

            [new OpLabel(0f, 0f, "Exceptions/Safeties:")],
            [
                DisableDuringCutscenesCheckbox = new OpCheckBox(DisableDuringCutscenes, 0f, 0f) { description = "THIS ONLY WORKS WITH JOLLY ENABLED. Yeah, no clue why they did it that way.\nDisables lasers during \"cutscenes\", as defined by Jolly; stuff like meeting Pebbles, the facility roots transition, or placing the rarefaction cell." },
                new OpLabel(0f, 0f, "Disable lasers during during \"cutscenes\"?") { description = "THIS ONLY WORKS WITH JOLLY ENABLED. Yeah, no clue why they did it that way.\nDisables lasers during \"cutscenes\", as defined by Jolly; stuff like meeting Pebbles, the facility roots transition, or placing the rarefaction cell." },
            ],
            [
                DisableDuringForcedInputCheckbox = new OpCheckBox(DisableDuringForcedInput, 0f, 0f) { description = "Disables lasers any time your avatar is loaded into the game, but they're controlled by something other than you. For instance, the Hunter/Artificer intros." },
                new OpLabel(0f, 0f, "Disable lasers during forced input?") { description = "Disables lasers any time your avatar is loaded into the game, but they're controlled by something other than you. For instance, the Hunter/Artificer intros." },
            ],
            [
                DisableNearPuppetsCheckbox = new OpCheckBox(DisableNearPuppets, 0f, 0f) { description = "Disables lasers when in the same room as any interator puppet. Technically talking to 5p is possible without this. Maybe even humanly viable." },
                new OpLabel(0f, 0f, "Disable lasers near iterator puppets?") { description = "Disables lasers when in the same room as any interator puppet. Technically talking to 5p is possible without this. Maybe even humanly viable." },
            ],
            [
                DisableDuringDialogueCheckbox = new OpCheckBox(DisableDuringDialogue, 0f, 0f) { description = "Disables lasers when any dialogue box is onscreen." },
                new OpLabel(0f, 0f, "Disable lasers during dialogue?") { description = "Disables lasers when any dialogue box is onscreen." },
            ],
        ];
        opTab.focusables.Clear();
        opTab.AddItems(BuildUIElements(UnbuiltPlayerOptions));

        RelaxedButton.OnPressDone += SetRelaxed;
        ChallengingButton.OnPressDone += SetChallenging;
        HellishButton.OnPressDone += SetHellish;

        AdjustOverseerSpawnsCheckbox.OnChange += ToggleSpawnrateTextboxes;
        GuaranteeWildOverseersCheckbox.OnChange += UpdateAllowEarlyOverseers;

        UpdateAllowEarlyOverseers();
        UpdateDisableDuringCutscenes();
    }
    public UIelement[] BuildUIElements(List<List<UIelement>> inputList, bool includeTitle = true)
    {
        static int RealModulo(int dividend, int divisor)
        {
            int remainder = dividend % divisor;               //C#'s % is not a modulo operator, it's a *remainder* operator, which will return negative values if the dividend is negative. That's stupid.
            return remainder + (remainder < 0 ? divisor : 0); //C# also doesn't have a Math.Mod() function to fill the hole created by % not being a true modulo. That's even stupider.
        }

        int firstRealElement = 1; //Don't count the scrollbox as part of the scrollbox contents.
        float verticalBuffer = 5f;
        float horizontalBuffer = 5f;
        float lineBreak = 10f;
        float opTextBoxWidth = 50f;
        Vector2 opHoldButtonSize = new(75f, 25f);

        float currentHeight = 0f;        //Normally we should start at our target height, but thanks to OpScrollBox we don't *know* what our target height is until AFTER generation.
        List<UIelement> outputList = []; //So, start at 0, work into the negatives, then just shove everything up when we're done. This is horrible but I don't see a better option.
        Vector2 scrollBoxEdgePadding = new(34f, 29f); //These magic numbers line up the title with the mod name text on the description screen.
        OpScrollBox scrollBox = new(new Vector2(-14f, -14f), new Vector2(628f, 628f), 0f, hasBack: false) { description = " "}; //More magic numbers, yeah yeah. Ask OpScrollBox where IT got them. This lines the borders up.
        outputList.Add(scrollBox);

        if (includeTitle) { inputList = [.. inputList.Prepend([new OpLabel(0f, 0f, mod.name + " Options", true)])]; } //"mod.name" pulls from the modinfo.json.


        //Place all of our elements.
        for (int i=0; i<inputList.Count; i++)
        {
            //Figure out how tall the current row is, and how far below the previous row it needs to be.
            float currentRowHeight = 0f;
            if (inputList[i].Count > 0)
            {
                for (int j = 0; j < inputList[i].Count; j++)
                {
                    currentRowHeight = Mathf.Max(currentRowHeight, inputList[i][j].size.y);
                }
                if (i > 0)
                {
                    currentHeight -= currentRowHeight + verticalBuffer;
                    if (inputList[i].First() is OpLabel) { currentHeight -= lineBreak; }
                }
            }
            //Position each element in the row, patching UIelement settings as we go.
            float currentDepth = scrollBoxEdgePadding.x;
            for (int j = 0; j < inputList[i].Count; j++)
            {
                UIelement currentElement = inputList[i][j];
                currentElement.pos = new Vector2(currentDepth, currentHeight - ((currentElement.size.y - currentRowHeight) / 2f));
                if (currentElement is OpTextBox)
                {
                    currentElement._size.x = opTextBoxWidth;
                    (currentElement as OpTextBox).accept = OpTextBox.Accept.Float;
                    (currentElement as OpTextBox).maxLength = Mathf.FloorToInt((currentElement.size.x - 20f) / LabelTest.CharMean(bigText: false)); //Apparently Change() doesn't update this. Caused a ton of pain and confusion.
                    currentElement.Change();
                }
                else if (currentElement is OpHoldButton)
                {
                    currentElement._size = opHoldButtonSize;
                    (currentElement as OpHoldButton).OnPressDone += (UIfocusable) => { currentElement.PlaySound(SoundID.MENU_Continue_From_Sleep_Death_Screen); };
                    currentElement.Change();
                }
                currentDepth += currentElement.size.x + horizontalBuffer;
                currentElement._AddToScrollBox(scrollBox);
                outputList.Add(currentElement);
            }
        }


        //Rebind EVERYTHING from scratch because _FocusCandidateCalculate can't even get STRAIGHT LINES correct.
        List<List<UIfocusable>> keybinderList = [];
        for (int i = 0; i < inputList.Count; i++)
        {
            keybinderList.Add(inputList[i].Where(element => element is UIfocusable).ToList().ConvertAll(element => element as UIfocusable));
        }
        keybinderList = [.. keybinderList.Where(element => element.Count > 0)];
        for (int i = 0; i < keybinderList.Count; i++)
        {
            for (int j = 0; j < keybinderList[i].Count; j++)
            {
                keybinderList[i][j].SetNextFocusable(UIfocusable.NextDirection.Left, keybinderList[i][RealModulo(j - 1, keybinderList[i].Count)]);
                keybinderList[i][j].SetNextFocusable(UIfocusable.NextDirection.Right, keybinderList[i][RealModulo(j + 1, keybinderList[i].Count)]);
                keybinderList[i][j].SetNextFocusable(UIfocusable.NextDirection.Up, keybinderList[RealModulo(i - 1, keybinderList.Count)][Math.Min(j, keybinderList[RealModulo(i - 1, keybinderList.Count)].Count - 1)]);
                keybinderList[i][j].SetNextFocusable(UIfocusable.NextDirection.Down, keybinderList[RealModulo(i + 1, keybinderList.Count)][Math.Min(j, keybinderList[RealModulo(i + 1, keybinderList.Count)].Count - 1)]);
            }
        }
        scrollBox.SetNextFocusable(UIfocusable.NextDirection.Left, keybinderList.First().First());  //I give up. I give up! I cannot find a single way to cleanly circumvent this idiotic "wrapper bodyblocks everything" issue,
        scrollBox.SetNextFocusable(UIfocusable.NextDirection.Right, keybinderList.First().First()); //at least without writing my own version of a scrollbox and quintupling the scope of this function. In all honesty,
        scrollBox.SetNextFocusable(UIfocusable.NextDirection.Up, keybinderList.First().First());    //this isn't really a bad workaround, but I got SO CLOSE to perfect behavior, and so many approaches were so close to a fix.


        //This is my emotional support scug. Please do not wake him. His functionality was a slog, but I love him anyway.
        FSprite scugSprite = new("slugcatSleeping") { width = 37f, height = 20f, x = 18.5f, y = 10f};
        OpContainer scugContainer = new(new((scrollBox.size.x - scrollBoxEdgePadding.x) - scugSprite.width, currentHeight))
        { size = new(scugSprite.width, scugSprite.height), description = "He sleeps, for he knows not of the horrors of building a remix menu." };
        scugContainer.container.AddChild(scugSprite);
        scugContainer._AddToScrollBox(scrollBox);
        outputList.Add(scugContainer);
        //OpSimpleButtons' minimum height is 4 pixels too tall and I don't see a way to shrink it. I'm completely out of patience to find a fix (if there even is one), so, hardcoded 2-pixel buffer is a feature now I guess.
        OpSimpleButton scugInteractor = new(new(scrollBox.size.x - scrollBoxEdgePadding.x - scugSprite.width - 2f, currentHeight - 2f), new(scugSprite.width + 4f, scugSprite.height + 4f))
        { mute = true, colorEdge = new(0,0,0,0), colorFill = new(0,0,0,0), description = "He sleeps, for he knows not of the horrors of building a remix menu." };
        scugInteractor.bumpBehav.Focused = false; //Because "zero alpha" actually only means "zero alpha unless and until hovered over". Why is doesBackBump not standard?
        scugInteractor.OnPressInit += (UIfocusable) => { scugContainer.PlaySound(SoundID.MENU_Dream_Switch); };
        scugInteractor._AddToScrollBox(scrollBox);
        outputList.Add(scugInteractor);
        keybinderList.Last().Last().SetNextFocusable(UIfocusable.NextDirection.Right, scugInteractor);
        scugInteractor.SetNextFocusable(UIfocusable.NextDirection.Up, keybinderList[RealModulo(keybinderList.Count - 2, keybinderList.Count)].Last());
        scugInteractor.SetNextFocusable(UIfocusable.NextDirection.Down, keybinderList.First().Last());
        scugInteractor.SetNextFocusable(UIfocusable.NextDirection.Left, keybinderList.Last().Last());
        scugInteractor.SetNextFocusable(UIfocusable.NextDirection.Right, keybinderList.Last().First());


        //Dear OpScrollBox... How am I supposed to define the final height of your contents BEFORE I GENERATE YOU AND YOUR CONTENTS?!
        (outputList[0] as OpScrollBox).contentSize = Math.Abs(currentHeight) + (scrollBoxEdgePadding.y * 2) + outputList[firstRealElement].size.y;
        (outputList[0] as OpScrollBox).Change();
        (outputList[0] as OpScrollBox).ScrollToTop(); //WHY ISN'T THIS DEFAULT?!
        for (int i=firstRealElement; i<outputList.Count; i++)
        {
            outputList[i].PosY += Math.Abs(currentHeight) + scrollBoxEdgePadding.y;
            outputList[i].Change();
        }


        return [.. outputList];
    }
    public override void Update()
    {

    }
}
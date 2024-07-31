# guidebook reagent skill effects

reagent-effect-guidebook-Helm-modifier =
    { $chance ->
        [1] Modifies
        *[other] modify
    } Helm by {$Helm} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)}
reagent-effect-guidebook-Weapons-modifier =
    { $chance ->
        [1] Modifies
        *[other] modify
    } Weapons by {$Weapons} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)}
reagent-effect-guidebook-MechanicalEngineering-modifier =
    { $chance ->
        [1] Modifies
        *[other] modify
    } MechanicalEngineering by {$MechanicalEngineering} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)}
reagent-effect-guidebook-ElectricalEngineering-modifier =
    { $chance ->
        [1] Modifies
        *[other] modify
    } ElectricalEngineering by {$ElectricalEngineering} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)}
reagent-effect-guidebook-Medical-modifier =
    { $chance ->
        [1] Modifies
        *[other] modify
    } Medical by {$Medical} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)}

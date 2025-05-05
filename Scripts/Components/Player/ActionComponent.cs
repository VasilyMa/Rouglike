namespace Client {
    struct ActionComponent {
        public ActionTypes ActionType;
    }
}
public enum ActionTypes{
    None, AttackDown, AttackUp, SpecAttackDown, SpecAttackUp, Dash, ProjectTileAbilityDown, ProjectTileAbilityUp, UtilityAbilityDown, UtilityAbilityUp, Interaction
}
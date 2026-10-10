# Analyse et corrections — Dawg.Solver.Winform

Analyse en profondeur du projet WinForm (`Dawg.Resolver.Winform.Test`) et de son moteur,
suite au symptôme : *« BONJOUR est bien trouvé, mais AutoPlay 1 pose le mot n'importe comment
sur le plateau, et AutoPlay All ne fonctionne pas bien »*.

La recherche de coups (moteur DAWG) était correcte : les bugs étaient dans la **pose des lettres**
sur le plateau et dans la **gestion de l'état du plateau entre deux coups**.

## Bug 1 (critique) — La pose du mot n'avançait pas de case

**Fichier :** `View/FormTile.cs` — `SetWord`

`SetWord` appelait `SetRightOrDownLetter` sur `this` (la case de départ) **à chaque itération** :

```csharp
this.SetFirstLetter(text.First(), ...);
foreach (var c in text.Skip(1))
    this.SetRightOrDownLetter(c, ...);   // nextTile = this.RightTile → TOUJOURS la même case !
```

Résultat : seule la 1re lettre était posée à la bonne case ; toutes les suivantes s'écrasaient
sur la case `start+1`. Pour `BONJOUR`, le plateau n'affichait en réalité que `B` puis `R`
(dernière lettre écrite), d'où l'impression que « le mot se met mal ».

**Correction :** parcours case par case à partir de la case de départ, dans la direction du mot.
Une seule méthode (`SetLetterOnTile`) pose la lettre, synchronise la grille vivante (utile pour les
coups verticaux issus de la grille transposée) et ne retire du chevalet **que les lettres réellement
posées** (les lettres déjà présentes sur le plateau — mots croisés — ne sont plus consommées).

Le retrait de lettres en double dans `Game.PreviewWord` a été supprimé : `SetWord` est désormais la
seule source de vérité pour la consommation du chevalet.

## Bug 2 (critique) — Le plateau était effacé à chaque recherche

**Fichier :** `Model/Game.cs` — `CleanTiles`

`CleanTiles` (appelée avant chaque recherche via `DetectTiles`) effaçait **toutes** les lettres,
y compris les mots déjà validés, **et** remettait tous les multiplicateurs de cases bonus à 1.
Comme `BackupGameGrid` sauvegarde des **références** vers les mêmes objets, la sauvegarde était
corrompue en même temps. Dès le 2e coup, le plateau se retrouvait vide et sans bonus, et
`IsFirstMove` redevenait vrai (case centrale vide) → AutoPlay All rejouait sans cesse au centre.

**Correction :** `CleanTiles` ne nettoie plus que l'état transitoire de recherche (ancres,
contrôleurs) et les lettres **non** validées (aperçus). Les lettres jouées et les multiplicateurs
sont conservés d'un coup à l'autre.

### Conséquence : une vraie réinitialisation de partie

Comme le plateau n'est plus effacé « par accident », une nouvelle méthode `Game.ResetGame()`
réinitialise explicitement la partie (sac, coups, scores) **et** vide le plateau (en conservant les
multiplicateurs). Elle est appelée par `MainForm.NewGame(..., initUI: true)`, qui remet aussi à zéro
le texte et la couleur des cases affichées.

## Bug 3 — `GetWordFromTile` / `GetNextTile` : NullReferenceException

**Fichier :** `View/FormTile.cs`

- `GetNextTile(direction, ref string)` : condition booléenne erronée
  (`... && ... || !tileToCheck.IsAnchor || ...`) provoquant une `NullReferenceException`
  dès que la case suivante était `null` (bord du plateau).
- `GetWordFromTile` créait un `Word` sans `StartTile`, puis appelait `SetText` → `NullReference`.

**Correction :** `GetWordFromTile` remonte au début du mot (`WordMostLeftTile` / `WordUpperTile`),
lit les lettres contiguës jusqu'à la première case vide, renvoie un `Word` correctement initialisé
(ou `null` si < 2 lettres). `GetNextTile(ref string)` est protégé contre le dépassement de plateau.
Ceci rend utilisables le bouton **Validate**, le **double-clic** (définition) et la touche **Entrée**.

Au passage, `MainForm.btnValidate_Click` lisait `LastPlayedTile` (propriété du formulaire, jamais
affectée) au lieu de `game.LastPlayedTile` → le bouton Validate ne faisait rien. Corrigé.

## Mise à jour NuGet

`System.Collections.Immutable` : **6.0.0 → 8.0.0** dans `Dawg.Solver.Winform.csproj` et
`DawgResolver/Dawg.Solver.csproj` (8.0.0 = choix sûr et éprouvé sur .NET Framework 4.8 ;
la 9.0.0 est aussi compatible via netstandard2.0).

## Fichiers modifiés

- `Dawg.Resolver.Winform.Test/View/FormTile.cs`
- `Dawg.Resolver.Winform.Test/Model/Game.cs`
- `Dawg.Resolver.Winform.Test/Forms/MainForm.cs`
- `Dawg.Resolver.Winform.Test/Dawg.Solver.Winform.csproj`
- `DawgResolver/Dawg.Solver.csproj`

## À vérifier côté Windows (non compilable dans l'environnement d'analyse)

Le projet est en .NET Framework 4.8 / WinForms : il faut le compiler et le tester dans Visual Studio.
Scénarios de test recommandés :
1. Chercher et jouer `BONJOUR` (chevalet `JOUrBON`) → le mot complet doit s'afficher aligné.
2. **AutoPlay 1** plusieurs fois de suite → chaque mot s'ancre sur les précédents, scores cohérents.
3. **AutoPlay All** → la partie se déroule jusqu'à la fin sans effacer le plateau.
4. **New Game** → plateau vidé, bonus conservés, scores remis à zéro.

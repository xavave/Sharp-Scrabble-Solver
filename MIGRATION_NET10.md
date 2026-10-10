# Migration .NET Framework 4.8 → .NET 10 (SDK-style)

Migration manuelle de la **chaîne WinForm** : `Dawg` → `DawgResolver` → `Dawg.Solver.Winform`.
Les autres projets de la solution (WPF, Xamarin `Scrabbler.Universal`, consoles) ne sont **pas** touchés.

## Cibles

| Projet | Avant | Après |
|---|---|---|
| `Dawg` | net48 (old-style) | **net10.0** (SDK-style) |
| `DawgResolver` (`Dawg.Solver.csproj`) | net48 | **net10.0** |
| `Dawg.Solver.Winform` | net48, WinExe | **net10.0-windows**, `UseWindowsForms` |

## Changements de code nécessaires

1. **`Dawg/Dictionnaire.cs` — suppression de la dépendance WPF.**
   `System.Windows.Threading.Dispatcher` (assembly `WindowsBase`, donc WPF) servait uniquement à
   marshaler les événements de progression pendant la construction du DAWG. Remplacé par
   `SynchronizationContext` (runtime de base) via un helper `RunOnOwnerThread`. Même comportement
   (marshaling synchrone vers le thread créateur s'il a un contexte), mais `Dawg` devient **portable**
   (plus besoin de `net*-windows` ni de WPF).

2. **`Dawg/Dictionnaire.cs` — chargement du dico robuste.**
   `ChargerFichierDAWG` cherchait le fichier via un chemin relatif (répertoire de travail courant).
   Ajout d'un repli sur `AppContext.BaseDirectory` (dossier de l'exe) pour éviter un dico introuvable
   selon le mode de lancement.

3. **`Process.Start("https://…")` → `ProcessStartInfo { UseShellExecute = true }`**
   (`MainForm.cs`, `Word.cs`). Sur .NET moderne, `Process.Start` avec une URL lève une exception sans
   `UseShellExecute = true`.

## Points traités dans les `.csproj`

- **Ressources embarquées** : le SDK n'embarque pas les `.txt` automatiquement. Les plateaux
  (`initial_board*.txt`) et dictionnaires sont ré-embarqués explicitement
  (`<None Remove> + <EmbeddedResource Include="Resources\*.txt">`). Le code les lit par
  `EndsWith(nomfichier)`, donc le changement de préfixe de nom de ressource n'a pas d'impact.
- **`EN_Collins_dawg.txt`** : conservé en copie à côté de l'exe (`CopyToOutputDirectory=Always`),
  car `Dawg` le charge par fichier.
- **Ressources de formulaires** (`Forms\*.resx`, `View\*.resx`) : `EmbeddedResourceUseDependentUponConvention`
  activé pour conserver le nommage « classique » (par type), sinon les resx dans des sous-dossiers
  ne seraient pas retrouvés à l'exécution (`MissingManifestResourceException`).
- **`GenerateAssemblyInfo=false`** : on garde les `Properties\AssemblyInfo.cs` existants (sinon
  attributs d'assembly en double).
- **`Noeud1Temps.cs` / `Noeud2Etapes.cs`** : présents sur disque mais **jamais compilés** dans l'ancien
  projet `Dawg` ; exclus explicitement (`<Compile Remove>`) pour garder le même périmètre.
- **Packages** :
  - `System.Collections.Immutable` : **retiré** (intégré au runtime .NET moderne).
  - `System.Drawing` (réf. `DawgResolver`) : **retirée** (inutilisée).
  - `System.Configuration.ConfigurationManager` **ajouté** au projet WinForm : requis par
    `Properties\Settings.Designer.cs` (`System.Configuration` n'est plus dans le framework de base).
- **`App.config`** : l'élément `<startup>` spécifique à .NET Framework a été retiré.
- Les anciennes `<Reference>` (System, System.Xml, System.Windows.Forms, System.Net.Http, System.Data…)
  sont désormais implicites via le SDK + `UseWindowsForms`.
- Le fichier **solution n'a pas besoin d'être modifié** (le GUID de type projet C# reste compatible).

## À faire côté Windows (je ne peux pas compiler ici)

1. Ouvrir la solution dans **Visual Studio 2022 (17.12+)** avec le **SDK .NET 10** installé.
2. Restaurer + compiler les 3 projets (`Dawg`, `DawgResolver`, `Dawg.Solver.Winform`).
3. Si erreurs/avertissements de build → me les transmettre, je corrige.
4. Tester : recherche/pose de `BONJOUR`, `AutoPlay 1`, `AutoPlay All`, `New Game`, double-clic
   définition (ouverture navigateur), changement de dictionnaire/taille de plateau.

> Remarque : si l'ouverture de **Propriétés du projet → Paramètres** régénère `Settings.Designer.cs`,
> le package `System.Configuration.ConfigurationManager` (déjà ajouté) couvre le besoin.

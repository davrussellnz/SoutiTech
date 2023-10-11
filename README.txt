
  _____  ______          _____    __  __ ______ 
 |  __ \|  ____|   /\   |  __ \  |  \/  |  ____|
 | |__) | |__     /  \  | |  | | | \  / | |__   
 |  _  /|  __|   / /\ \ | |  | | | |\/| |  __|  
 | | \ \| |____ / ____ \| |__| | | |  | | |____ 
 |_|  \_\______/_/    \_\_____/  |_|  |_|______|
                                                
                                                

*Après la migration et la mise à jour de la base de données, veuillez exécuter le script :

SQLQuery1.sql

afin de remplir les menus déroulants Catégorie et Priorité (lors de la création et de la modification des tickets).

*Lors de l'enregistrement d'un utilisateur, cliquez avec le bouton droit de la souris sur le champ du mot de passe et sélectionnez "utiliser le mot de passe suggéré". Les exigences en matière de mot de passe sont strictes pour la bibliothèque d'identité. Par défaut, la bibliothèque d'identité applique les exigences suivantes en matière de mot de passe :

- Longueur minimale : Le mot de passe doit comporter au moins 6 caractères.
- Exiger des caractères non alphanumériques : Le mot de passe doit contenir au moins un caractère non alphanumérique (par exemple, un caractère spécial comme !, @, #, etc.).
- Exiger des lettres minuscules : Le mot de passe doit contenir au moins une lettre minuscule (a-z).
- Lettres majuscules obligatoires : Le mot de passe doit contenir au moins une lettre majuscule (A-Z).
- Chiffres obligatoires : Le mot de passe doit contenir au moins un chiffre (0-9).

Voici un exemple de mot de passe valide : 
4RGBFBj9tATimvG-m

*Avant de créer des tickets en tant que client, au moins un membre du support technique doit être inscrit pour que les tickets soient automatiquement assignés.

*Pour utiliser AI Chat :

Demandez un accès temporaire à CORS Anywhere à partir de ce lien :

https://cors-anywhere.herokuapp.com/corsdemo

Et cliquez sur le bouton qui dit :

"Request temporary access to the demo server"

*Pour se connecter en tant qu'administrateur :
u : admin@example.com
p : Admin@123

*Pour les tests unitaires, ouvrez le projet gestionticket_v2.Tests.csproj 
dans VSCode et mettez à jour :

<ItemGroup>
<ProjectReference Include="FILE PATH TO ...\gestionticket_v2\gestionticket_v2.csproj">
</ItemGroup>



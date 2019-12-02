Excel/VBA - Jouer au Scramble Duplicate---------------------------------------
Url     : http://codes-sources.commentcamarche.net/source/100473-excel-vba-jouer-au-scramble-duplicateAuteur  : carlvbDate    : 04/04/2014
Licence :
=========

Ce document intitulé « Excel/VBA - Jouer au Scramble Duplicate » issu de CommentCaMarche
(codes-sources.commentcamarche.net) est mis à disposition sous les termes de
la licence Creative Commons. Vous pouvez copier, modifier des copies de cette
source, dans les conditions fixées par la licence, tant que cette note
apparaît clairement.

Description :
=============

Bonjour à tous,    
<br />
<br />Voici le début de mon projet de scramble ;)  
  
<br />La méthode de recherche des coups est basée sur celle développée par A
ppel et Jacobson dans les années 80.    
<br />Le dictionnaire (fourni dans le 
zip) est présenté sous forme de DAWG.    
<br />Pour l'instant, il ne trouve pa
s encore tous les top coups mais je travaille la-dessus.    
<br />A terme, je 
voudrai faire un menu où on affronte l'ordi mais pour le moment, je souhaiterais
 recenser tous les bugs et les anomalies dans la recherche du meilleur coup.    

<br />
<br />Pour l'essayer:   
<br />- Charger le dictionnaire (fourni dans
 le zip)   
<br />- Lancer une nouvelle partie   
<br />- Générer un tirage (m
anuel ou par les boutons dédiés)   
<br />- Recherche les coups   
<br />- Sél
ectionner un coup dans la liste des coups trouvés   
<br />- Jouer le coup séle
ctionné   
<br />- et ainsi de suite...   
<br />
<br />Le contenu du sac est
 affiché dans une listbox tout comme l'historique de la partie.   
<br />
<br 
/>Le temps de recherche est limitée par défaut à 10 secondes mais la constante c
orrespondante peut être modifiée dans le code.   
<br />
<br />Le code est abo
ndament commenté mais je reste à votre disposition pour toute question.    
<br
 />
<br />Vos commentaires, suggestions et critiques sont les bienvenus ainsi q
ue les bugs du programme.    
<br />
<br />Merci d'avance. 
<br />
<br />Mis
e à jour : 
<br />- Correction de bugs et d'erreurs dans l'algortihme de recher
che des solutions (il est maintenant censé trouver tous les tops coups, la seule
 limite devrait être le temps alloué aux recherches limitées à 60 secondes par d
éfaut mais modifiable dans la déclaration des constantes) 
<br />- Limitation d
es solutions à garder pour l'affichage (Par défaut le top 30 mais également modi
fiable - Attention sur des cas de double joker dans le rack, on peut atteindre p
lus de 100 000 coups valides!!! ) 
<br />- Rajout d'une fonction de chargement 
de grille manuelle (pour continuer une précédente partie ou pour tester le progr
amme face à des applications similaires 
<br />- Rajout d'une vérification de l
a présence d'un mot dans le dictionnaire. 
<br />- Mise en exergue sur la grill
e des nouvelles tuiles placées.
<br />- plus quelques modifications mineures

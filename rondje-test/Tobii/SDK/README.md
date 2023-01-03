# Tobii Unity SDK for Desktop

- This package uses materials made for Built-in Render Pipeline. When using URP please don't forget to upgrade the materials using Edit > Render Pipeline > Universal Render Pipeline -> Upgrade Project Materials to URP Materials.
- BulletholeMaterial and LaserSightImpact decal materials might lose alpha transparency after an automatic upgrade.
- Highlight material in the Action Game sample doesn't support URP. You will still be able to select the objects in this sample but with no highlight visualization.
- Action Game First Person sample is using stacked cameras for weapon visualization. To use with URP please change Render Type to Overlay for the WeaponCamera and add WeaponCamera to the Stack of the MainCamera.
- Dynamic light adaptation sample is made for Built-in Render Pipeline and doesn't support URP.

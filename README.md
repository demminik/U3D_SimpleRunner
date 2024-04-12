Very basic demo with pure C# and Unity3D tools:
 - no reactive packages, no injections.
 - presentation and logic split (logic is not extracted to non-MonoBeh-classes)
 - intentionally grotesque facade that goes through all objects (normal people would do this as injection, but there are no injections in this project)
 - custom and fully controllable time management
 - configs provider that is not dependent on configs implementation
 - level generator that allows generation of levels from tiles of any complexity
 - unit skins system

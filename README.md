PREREQUISITES
SP1,NuGet, SQLCompact tools 

Bootstrapping
Setup connection string


Topics

* Simple convention mapping
  * Define classes
  * Save
  * View in SQL CE
* Complex property
   * When model changes we can see DatabaseInitializers
   * By default complex type goes in same table as model with Property_ prefix on 
   * Complex type is required
   * Can initialize and write null checks in ctor of owning entity
   * Complex type should be discovered but you can always denote using attribute [ComplexType] or modelBuilder.ComplexType<Address>()
* HasMany
  * Properties will just silently not work if non-virtual
  * Notice the generated column name Table_ID
    * Can fix this up in attributes by delcaring property on the other side of the model
    * Have to do the ID and ForeignKey as 2 separate properties.
  * EfProf points out a select N+1 problem when looping over customers and checking children.
    * Can get rid of alert by using Include
  * By default can not remove parent without removing children
  * Cant really do much config via the attributes.
  * After the HasMany() the next calls describe the relation ship on the <child> object
* Nullability of property is taken into account when schema is created
  
* Can generate edmx from database still
  * To use sql ce 4 database you have to do it from web project
    

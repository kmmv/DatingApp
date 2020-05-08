15 Adding the 'Likes' functionality from start to finish
***

152 Configuring the EF relationship so users can like each other
***
Until now we use navigation property
Now we need kind to  Many to Many  relationship
  - User hasOne Like(r) With Many Like(es)
We use Fluent API

153 Creating the Like entity
***
*. On the API create Like Model
*. goto Users.cs add Likers and Likees virtual property
*. goto DataContext.cs add public DbSet<Like>  Likes {get;set;}
*. on the   DataContext.cs override OnModelCreating and add the relationships
*. dotnet ef migrations add AddedLikeEntity - this will create AddedLikeEntity migrations
*. dotnet ef database update

- Migration error
*. SQLite does not support this migration operation ('AlterColumnOperation'). For more information
        - Have removed the column referencing photoid and issues dotnet ef database update

154 Adding the Send Like functionality to the SPA
****
*. On the API implement GetLike on the DatingRepository
*. On the UserController implement the LikeUser HttpPost method
*. Test on PostMan

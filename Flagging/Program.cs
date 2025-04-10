using ConsoleTables;
using Flagging;
using Flagging.Data;
using System.Collections.Generic;

var dbContext = new DataContext();
dbContext.Seed();
dbContext.SaveChanges();

// #
// the database contains: 
// 1. users
// 2. articles
// 3. comments
// 4. flags -> A flag is an instance where someone reported a user/article/comment as offensive for review
//
//      +---------+            +---------+            +----------+
//      | Article |            | User    |            | Comment  |
//      +---------+            +---------+            +----------+
//      | UserId  |< -------- >| Id      |< -------- >| UserId   |
//      | Id      |            | First   |            | Id       |
//      | Desc    |            | Last    |            | Message  |
//      +---------+            +---------+            +----------+
//          |                       |                       | 
//          |  o--------------------o                       |
//          |  |    +-----------------------------------+   |
//          |  |    | Flag                              |   |
//          |  |    +-----------------------------------+   |
//          |  |    | Id                                |   |
//          |  o--->| ReportedUserId (FK->User.Id)      |   |
//          o------>| ArticleId (FK->Article.Id)        |   |
//                  | CommentId (FK->Comment.Id)        |<--o
//                  | Type [Article | Comment | Member] |
//                  | Status                            |
//                  | DateCreated                       |
//                  +-----------------------------------+
//
// ##
// This is an example of the data in the Flags table:
// +----+-----------------+-----------+-----------+---------------------+---------------------+
// | Id | ReportedUserId  | ArticleId | CommentId | Type                | DateCreated         |
// +----+-----------------+-----------+-----------+---------------------+---------------------+
// | 1  | NULL            | 1         | NULL      | Article             | 2024-01-01          |
// | 2  | NULL            | 1         | NULL      | Article             | 2024-02-02          |
// | 3  | 1               | NULL      | NULL      | Member              | 2024-03-03          |
// | 4  | 2               | NULL      | NULL      | Member              | 2024-04-04          |
// | 5  | 3               | NULL      | NULL      | Member              | 2024-05-05          |
// | 6  | NULL            | NULL      | 3         | Comment             | 2024-06-06          |
// | 7  | NULL            | NULL      | 3         | Comment             | 2024-07-07          |
// | 8  | NULL            | NULL      | 4         | Comment             | 2024-08-08          |
// | 9  | 5               | NULL      | NULL      | Member              | 2024-09-09          |
// | 10 | NULL            | NULL      | 5         | Comment             | 2024-10-10          |
// +----+-----------------+-----------+-----------+---------------------+---------------------+
//
// ###
// [ ] the goal of this excercise is to create a query that would pull flags from the database.
// [ ] each row in the table that will be written in the Console represents all flags that were ever created for a single user/article/comment
// 
// ####
// [ ] have most recent flags at the top
// [ ] filter by keyword (match on user name, article description and comment message)
// [ ] implement paging
//
// #####
// Example Output: 
// +--------+----------+------------------+------------+------------------+
// | ItemId | ItemType | ItemDescription  | FlagCounts | DateLastFlagged  |
// +--------+----------+------------------+------------+------------------+
// | 1      | Article  | Article 1        | 2          | 2024-02-02       |
// | 1      | Member   | User 1           | 1          | 2024-03-03       |
// | 2      | Member   | User 2           | 1          | 2024-04-04       |
// | 3      | Member   | User 3           | 1          | 2024-05-05       |
// | 3      | Comment  | Comment 3        | 2          | 2024-07-07       |
// | 4      | Comment  | Comment 4        | 1          | 2024-08-08       |
// | 5      | Comment  | Comment 5        | 1          | 2024-09-09       |
// +--------+----------+------------------+------------+------------------+

// write code here

var query = dbContext.Flags;
var flaggedItems = query
    .Select(x => new FlaggedItemRow
    {

    });

WriteTable(flaggedItems.ToList());

Console.ReadKey();

void WriteTable(IEnumerable<FlaggedItemRow> flaggedItems)
{
    ConsoleTable
        .From<FlaggedItemRow>(flaggedItems)
        .Configure(o => o.NumberAlignment = Alignment.Right)
        .Write(Format.Alternative);
}
# Chat
A Chat application backend with rest API and websocket technologies in asp.net framework

Some instruction to APIs

Auth/Login
- This API excepts an username and a password and return a JWT token if login details was correct

Auth/Registration
- This API excepts an username, password, firstname, lastname, and an email (these fields are required) and an optional parameter lastname. 

User/Search
- Protect by JWT authorization
- This API excepts a keyword from HEADER and return a list of users where username, firstname, lastname, middlename or email contain the keyword

User/Friends
- Protect by JWT authorization
- Return a list of your friends

User/FriendRequests
- Protect by JWT authorization
- Return your got friend request

User/SendFriendRequest
- Protect by JWT authorization
- This API excepts from BODY the address's username to send a friend request him

User/AcceptFriendRequest
- Protect by JWT authorization
- This API excepts a friend request id and modify the status of it to accept 

User/DeclineFriendRequest
- Protect by JWT authorization
- This API excepts a friend request id and modify the status of it to denied 

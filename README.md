# PokerPrototype

The home controller will fetch either landing.cshtml or lobby.cshtml, depending on session["id"]. The layout file contains the code for the login modal. When the login button is pressed, it will send an ajax request to the login controller. The login controller will create a LoginModel.cs object, set the session variable if successful, and return a JSON. The client will either display an error, or redirect to the lobby. Currently, it only works when username is josh, and password is password. 

The layout file requires a section called topright, which will either be a login button, or an account info dropdown. I think it's a bad design to require this for every view we create, but I don't know how to pass a model to the layout. 

I'd like to delete the various authentication code files, but I kept them in just in case any of you know how to utilize it. I'd especially like to clean up loginpartial

TO DO:
- Add database access to the LoginModel constructor.
- Add a form to the signup view, and handle it in the signupController
- Add stuff to the lobby view, and make it do ajax requests similar to what I did with the loginController


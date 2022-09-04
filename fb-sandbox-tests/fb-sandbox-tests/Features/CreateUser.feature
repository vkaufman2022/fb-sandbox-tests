@createSingleUser
Feature: Create User Test Set

@successfullyCreated
Scenario Outline: Verify that user with different name and balance successfully created
	Given new users details
	| name   | balance   |
	| <name> | <balance> |
	When 1 users successfully created 
	Then the following response was got
	| name   | balance   |
	| <name> | <balance> |
Examples: 
| name | balance |
# same name and balances
| Sam  | 1000    |
| Sam  | 1000    |
# zero balance
| Jhon | 0       |
# special characters in name
| @my  | 100     |
# name is a number
| 100  | 200     |
# name is empty
|      | 300     |

@severalUsers
Scenario: Verify that several users successfully created
	Given new users details
	| name               | balance |
	| Edgar Allan Poe    | 100     |
	| Herman Melville    | 200     |
	| Walt Whitman       | 300     |
	| Mark Twain         | 400     |
	| T.S. Eliot         | 500     |
	| William Faulkner   | 600     |
	| Tennessee Williams | 700     |
	| Kurt Vonnegut      | 800     |
	| Emily Dickinson    | 900     |
	| Joseph Heller      | 1000    |
	When 10 users successfully created 
	Then the following response was got
	| name               | balance |
	| Edgar Allan Poe    | 100     |
	| Herman Melville    | 200     |
	| Walt Whitman       | 300     |
	| Mark Twain         | 400     |
	| T.S. Eliot         | 500     |
	| William Faulkner   | 600     |
	| Tennessee Williams | 700     |
	| Kurt Vonnegut      | 800     |
	| Emily Dickinson    | 900     |
	| Joseph Heller      | 1000    |

@100Users
Scenario: Verify that 100 users successfully created
	When provided 100 valid users details
	Then 100 users successfully created 

@unauthorized
Scenario: Verify that unauthorized operator could not create user
	Given new users details
	| name | balance |
	| Sam  | 100     |
	When unauthorized operator with email vk@gmail.com creates user
	Then the following error got : status code=401, token=vk@gmail.com, msg=Bad auth token

@invalidUserDetails
Scenario Outline: Verify that user with invalid name or balance could not be created 
	Given new users details
	| name   | balance   |
	| <name> | <balance> |
	When user failed to create
	Then the following error got - status code=<code>, msg=<message>
Examples:	
| name | balance | code | message                     |
# negative balance
| Joe  | -1000   | 400  | Balance can not be negative |
# decimal balance
| Ann  | 1.3     | 500  | Internal server error       |
# balance char
| Amy  | million | 500  | Internal server error       |  
@updateUser
Feature: Update User Test Set

Background: 
Given new users details
	| name   | balance |
	| Sam123 | 3000    |
	When 1 users successfully created

@updateNameAndBalance
Scenario Outline: Verify that name and balance can be successfully updated for existing user
    When update the user as follows
	| name   | balance   |
	| <name> | <balance> |
	Then the following response was got
	| name   | balance   |
	| <name> | <balance> |

	Examples: 
	| name   | balance |
	# update only name
	| Samuel | 3000    |
	# update only balance
	| Sam123 | 100     |
	# update name and balance
	| Ann    | 2000    |

@unauthorized
Scenario: Verify that unauthorized operator could not update user
	When unauthorized operator with email vk@gmail.com updates user
	| name   | balance |
	| Samuel | 3000    |
	Then the following error got : status code=401, token=vk@gmail.com, msg=Bad auth token
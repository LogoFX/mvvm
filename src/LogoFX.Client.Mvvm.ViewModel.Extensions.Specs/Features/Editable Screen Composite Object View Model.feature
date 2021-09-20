Feature: Editable Screen Composite Object View Model
	As an app developer
	I would like the framework to properly manage the state after the model changes
	So that I am able to develop apps faster

Scenario: Complex flow on composite view model should leave the model in consistent state
	When I open the application
	And I use editable screen composite object view model	
	And I add phone 645
	And I apply the changes
	And I add phone 347
	And I cancel the changes
	Then The model should contain correct phones

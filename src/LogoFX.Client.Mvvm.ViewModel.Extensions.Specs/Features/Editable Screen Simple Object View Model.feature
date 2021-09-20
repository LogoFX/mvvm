Feature: Editable Screen Simple Object View Model
	As an app developer
	I would like the framework to properly manage the state after the model changes
	So that I am able to develop apps faster

Scenario: Changing property's value during editing should raise notifications	
	When I open the application
	And I use editable screen simple object view model
	And I set the name to be a valid name
	Then A dirty notification is raised
	And A changes cancellation notification is raised

Scenario: Closing view model after property value has been changed should display a message
	When I open the application
	And I use editable screen simple object view model
	And I set the name to be a valid name
	And I close the editable screen object view model
	Then A message is displayed	

Scenario: Closing view model after property value has not been changed should not display a message
	When I open the application
	And I use editable screen simple object view model
	And I close the editable screen object view model
	Then A message is not displayed	

Scenario: Confirming changes on closing view model should save model
	When I open the application
	And I use editable screen simple object view model
	And I set all confirmation to 'Yes'
	And I set the name to be a valid name
	And I close the editable screen object view model
	Then The editable screen object view model is not dirty
	And The model is not dirty
	And The changes in the model are saved

Scenario: Discarding changes on closing view model should not save model
	When I open the application
	And I use editable screen simple object view model
	And I set all confirmation to 'No'
	And I set the name to be a valid name
	And I close the editable screen object view model
	Then The editable screen object view model is not dirty
	And The model is not dirty
	And The changes in the model are not saved

Scenario: Cancelling close on closing view model should not clear dirty status
	When I open the application
	And I use editable screen simple object view model
	And I set all confirmation to 'Cancel'
	And I set the name to be a valid name
	And I close the editable screen object view model
	Then The editable screen object view model is dirty
	And The model is dirty
	And The changes in the model are saved
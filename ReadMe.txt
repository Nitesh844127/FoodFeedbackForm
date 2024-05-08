
Food Feedback Form README
1. Serial Number Generation
Functionality: Automatically generates a serial number in the format yyyymmdd### where yyyymmdd represents the current date and ### represents a sequential number starting from 001.
Implementation: The GenerateSerialNumber() method in the FeedbackController class utilizes the current date and the count of existing feedback entries for the day to generate the serial number.
2. Name Validation
Functionality: Validates that the name provided is valid and not one of the restricted names such as "pqr", "xyz", or "aaa".
Implementation: Uses the RegularExpression attribute in the Feedback model to enforce the validation rule.
3. Phone Number Validation
Functionality: Validates that the provided phone number is a valid Indian mobile number and not sequential.
Implementation: Utilizes the RegularExpression attribute in the Feedback model to ensure the phone number matches the specified pattern.
4. Email Validation
Functionality: Validates that the provided email address is valid by calling an external API (https://rapidapi.com/mr_admin/api/email-verifier).
Implementation: Utilizes the EmailAddress attribute in the Feedback model for basic validation. Advanced validation can be implemented by integrating with the specified API.
5. Visiting Status
Functionality: Captures whether the feedback is from a first-time visitor or not.
Implementation: Utilizes radio buttons in the feedback form to allow the user to select either "Yes" or "No".
6. Food Liked Selection
Functionality: Allows users to select multiple food items they liked from a predefined list.
Implementation: Utilizes checkboxes in the feedback form for each food item, and the selected items are stored as a comma-separated string in the foodLiked property of the Feedback model.
7. Preferred Time Slot
Functionality: Allows users to select their preferred time slot from options: Morning, Afternoon, Evening, Late night.
Implementation: Utilizes a dropdown list in the feedback form to allow users to select their preferred time slot.
8. Photo Upload
Functionality: Allows users to upload their photo along with the feedback.
Implementation: Includes a file input field in the feedback form (Image) to enable users to upload their photo.
9. Saving Feedback and Search
Functionality: Saves the feedback form entries and provides a searchable list of feedback entries.
Implementation: The Create action method in the FeedbackController saves the feedback entries to the database. The Index action method retrieves and displays the feedback entries based on applied filters.
Additional Functionalities
SweetAlert Notification: Displays a notification using SweetAlert when feedback is successfully saved.
Filtering: Allows users to filter feedback entries based on time slot, food liked, and visiting status.

Tools used:

Visual studio 2022 for development
framework .net 8
SQLite dbBrowser for database 
chrome for windows localhost run
IIS express for run on browser
Dapper for ORM (object relational mapper)
Testing manual by filling entries
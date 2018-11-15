package jj47.cs262.calvin.edu.simplecalculator;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

/**
 * Main Activity class for the Simple Calculator Application.
 * <p>
 * Performs basic arithmetic operations based on user selection.
 * <p>
 * Supports multiply, divide, add, and subtract.
 */
public class MainActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener, View.OnClickListener {

    private TextView result;
    private Spinner opSelect;
    private EditText editValue1, editValue2;
    private String opSelected;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Find all Activity Objects.
        // Class variables.
        Button calculate = findViewById(R.id.Button_Calculate);
        result = findViewById(R.id.TextView_Result);
        opSelect = findViewById(R.id.Spinner_OpType);
        editValue1 = findViewById(R.id.EditText_Value1);
        editValue2 = findViewById(R.id.EditText_Value2);

        // Set button listener.
        calculate.setOnClickListener(this);
    }

    /**
     * Method sets up a spinner for operation selection.
     */
    @Override
    protected void onStart() {

        super.onStart();

        // Setup spinner for operator selection.

        // Create an ArrayAdapter using the string array and a default spinner layout.
        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this, R.array.op_array, android.R.layout.simple_spinner_item);

        // Specify the layout to use when the list of choices appears
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        // Apply the adapter to the spinner
        opSelect.setAdapter(adapter);

        // Set the event listener for the spinner widget.
        opSelect.setOnItemSelectedListener(this);
    }

    /**
     * Method that listens for the user selection of a spinner item.
     * (When user selects an item using the spinner)
     *
     * @param parent AdapterView object.
     * @param view   View object.
     * @param pos    integer value.
     * @param id     integer value.
     */
    public void onItemSelected(AdapterView<?> parent, View view,
                               int pos, long id) {
        // Retrieve selected item and convert to string.
        opSelected = parent.getItemAtPosition(pos).toString();
    }

    /**
     * Method stub that is necessary for implements.
     * (When user does not select an item using the spinner.)
     *
     * @param parent AdapterView object.
     */
    public void onNothingSelected(AdapterView<?> parent) {
        // Another interface callback
    }

    /**
     * Method that reacts to the user clicking the button.
     * Performs the arithmetic operation based on operator selected.
     * (Do something once user clicks button.)
     *
     * @param v View object.
     */
    @Override
    public void onClick(View v) {

        Toast.makeText(this, "Calculating!", Toast.LENGTH_SHORT).show();

        String number1 = editValue1.getText().toString();
        String number2 = editValue2.getText().toString();

        switch (opSelected) {
            case "add":
                try {
                    int addition = Integer.parseInt(number1) + Integer.parseInt(number2);
                    result.setText(String.valueOf(addition));
                } catch (Exception excpt) {
                    result.setText(R.string.error_invalid_number);
                }
                break;
            case "subtract":
                try {
                    int subtraction = Integer.parseInt(number1) - Integer.parseInt(number2);
                    result.setText(String.valueOf(subtraction));
                } catch (Exception excpt) {
                    result.setText(R.string.error_invalid_number);
                }
                break;
            case "multiply":
                try {
                    int multiplication = Integer.parseInt(number1) * Integer.parseInt(number2);
                    result.setText(String.valueOf(multiplication));
                } catch (Exception except) {
                    result.setText(R.string.error_invalid_number);
                }
                break;
            case "divide":
                try {
                    int division = Integer.parseInt(number1) / Integer.parseInt(number2);
                    result.setText(String.valueOf(division));
                } catch (Exception except) {
                    result.setText(R.string.error_invalid_number);
                }
                break;
            default:
                break;
        }
    }
}

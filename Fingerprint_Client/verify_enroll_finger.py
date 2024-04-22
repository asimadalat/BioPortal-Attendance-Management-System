# ANYTHING I HAVE NOT COMMENTED is from the fingerprint_simpletest_rpi.py script
# In Examples folder here: https://github.com/adafruit/Adafruit_CircuitPython_Fingerprint.git
# I have also added sections from fingerprint_template_file_compare.py
# Where I have done this, is indicated in the comments

# Local database manipulation over local network
import mysql.connector

import time
from datetime import datetime
import serial
import adafruit_fingerprint

# Host IP, user, pass, and db to access
connection = mysql.connector.connect(
    host="hostname",
    user="username",
    password="password",
    database="databasename"
)

uart = serial.Serial("/dev/ttyUSB0", baudrate=57600, timeout=1)

finger = adafruit_fingerprint.Adafruit_Fingerprint(uart)

def reset_sensor(uart, finger):
    ''' Resets fingerprint sensor by closing and re-establishing connection

        Parameters: uart (external connection), finger (library instance)
        Returns : uart, finger with values reset '''
    
    uart.close()
    time.sleep(1)
    uart = serial.Serial("/dev/ttyUSB0", baudrate=57600, timeout=1)
    finger = adafruit_fingerprint.Adafruit_Fingerprint(uart)
    return uart, finger


def get_fingerprint():
    print("Waiting for image...")
    while True:
        print(".")
        if finger.get_image() != adafruit_fingerprint.OK:
            continue
        if finger.image_2_tz(1) != adafruit_fingerprint.OK:
            continue

        # connection = sqlite3.connect("students.db") # Connect to existing db
        connection.reconnect() # Reopens connection
        cursor = connection.cursor() # Define cursor
        cursor.execute("SELECT id, fingerprint FROM StudentsPM") # Execute query
        students = cursor.fetchall() # Assign returned list to variable
        connection.close() # Close connection

        for student in students: # For each fingerprint
            # Extract relevant data from tuple, convert fingerprint to binary form
            print(student)
            id_number, fingerprint_data_hex = student
            fingerprint_data = bytearray.fromhex(fingerprint_data_hex)
            print(fingerprint_data_hex)
            print(fingerprint_data)
            # Send print to sensor at temp location 2 for comparison
            finger.send_fpdata(fingerprint_data, "char", 2)
            
            #---------fingerprint_template_file_compare.py-------
            if finger.compare_templates() == adafruit_fingerprint.OK:
                set_led_local(color=2, speed=150, mode=6)
                print(f"Detected #{id_number} with confidence {finger.confidence}")
                update_attendance(id_number, 1)
                return True

        print("Finger not found")
        return False
    
    
def update_attendance(studentid, present):
    ''' Updates attendance by updating value of present corresponding
        to id of matching fingerprint, using MySQL

        Parameters: studentid (student to update), present (value to update to)
        Returns : N/A '''
    connection.reconnect() # Reopens connection
    cursor = connection.cursor() # Define cursor
    table = "StudentsAM" if datetime.now().hour < 12 else "StudentsPM" # Change table based on time of day
    print(f"UPDATE {table} SET present = {present} WHERE id = {studentid}") # Visual feedback of id
    cursor.execute(f"UPDATE {table} SET present = %s WHERE id = %s", (present, studentid)) # Execute query
    connection.commit()
    connection.close() # Close connection


def enroll_finger(location):
    for fingerimg in range(1, 3):
        if fingerimg == 1:
            print("Place finger on sensor...", end="")
        else:
            print("Place same finger again...", end="")

        while True:
            i = finger.get_image()
            if i == adafruit_fingerprint.OK:
                print("Image taken")
                break
            if i == adafruit_fingerprint.NOFINGER:
                print(".", end="")
            elif i == adafruit_fingerprint.IMAGEFAIL:
                print("Imaging error")
                return False
            else:
                print("Other error")
                return False

        print("Templating...", end="")
        i = finger.image_2_tz(fingerimg)
        if i == adafruit_fingerprint.OK:
            print("Templated")
        else:
            if i == adafruit_fingerprint.IMAGEMESS:
                print("Image too messy")
            elif i == adafruit_fingerprint.FEATUREFAIL:
                print("Could not identify features")
            elif i == adafruit_fingerprint.INVALIDIMAGE:
                print("Image invalid")
            else:
                print("Other error")
            return False

        if fingerimg == 1:
            print("Remove finger")
            time.sleep(1)
            while i != adafruit_fingerprint.NOFINGER:
                i = finger.get_image()

    print("Creating model...", end="")
    i = finger.create_model()
    if i == adafruit_fingerprint.OK:
        print("Created")
    else:
        if i == adafruit_fingerprint.ENROLLMISMATCH:
            print("Prints did not match")
        else:
            print("Other error")
        return False

    print("Downloading template...")
    data = finger.get_fpdata("char", location) # Get data at assigned location
    print(data)
    fingerprint_data = bytearray(data).hex()  # Hex should be easier to store
    print(fingerprint_data)

    # connection = sqlite3.connect("students.db") # Connect to db
    connection.reconnect() # Reopens connection
    cursor = connection.cursor() # Define cursor
    
    # Insert enrolled fingerprint at correct location
    cursor.execute("UPDATE StudentsPM SET fingerprint = %s WHERE id = %s", (fingerprint_data, location))
    connection.commit() # Commits change
    connection.close() # Closes connection

    #---------fingerprint_template_file_compare.py-------
    set_led_local(color=2, speed=150, mode=6) # Visual feedback for successfully downloaded template
    print("Template is saved in the database.")

#---------fingerprint_template_file_compare.py-------
def set_led_local(color=1, mode=3, speed=0x80, cycles=0):
    try:
        finger.set_led(color, mode, speed, cycles)
    except Exception as exc:
        print("INFO: Sensor les not support LED. Error:", str(exc))





def get_num():
    ''' Gets the maximum student ID number and and asks to
        input number between 1 and minimum.
        
        Parameters : N/A
        Returns: (str) Enter ID prompt '''
    
    connection.reconnect() # Reopens connection 
    cursor = connection.cursor() # Define cursor
    cursor.execute("SELECT MAX(id) FROM StudentsPM") # Execute query
    max_id = "".join(cursor.fetchone()) # Tuple to string
    connection.close() # Close connection
    try:
        i = int(input("Enter Student ID # from 0-{}: ".format(max_id)))
    except ValueError:
        pass
    return i


while True:
    print("----------------")
    if finger.read_templates() != adafruit_fingerprint.OK:
        raise RuntimeError("Failed to read templates")
    print("Fingerprint templates: ", finger.templates)
    if finger.count_templates() != adafruit_fingerprint.OK:
        raise RuntimeError("Failed to read templates")
    print("Number of templates found: ", finger.template_count)
    if finger.set_sysparam(6, 2) != adafruit_fingerprint.OK:
        raise RuntimeError("Unable to set package size to 128!")
    if finger.read_sysparam() != adafruit_fingerprint.OK:
        raise RuntimeError("Failed to get system parameters")
    print("Size of template library: ", finger.library_size)
    print("e) enroll print")
    print("f) find print")
    print("d) delete print")
    # Removed save_fingerprint_image
    print("r) reset library")
    print("q) quit")
    print("----------------")
    c = input("> ")

    if c == "e":
        enroll_finger(get_num())
        uart, finger = reset_sensor(uart, finger) # Reset sensor for next use
    if c == "f":
        get_fingerprint()
        uart, finger = reset_sensor(uart, finger) # Reset sensor for next use
    if c == "d":
        if finger.delete_model(get_num(finger.library_size)) == adafruit_fingerprint.OK:
            print("Deleted!")
        else:
            print("Failed to delete")
    if c == "r":
        if finger.empty_library() == adafruit_fingerprint.OK:
            print("Library empty!")
        else:
            print("Failed to empty library")
    if c == "q":
        print("Exiting fingerprint example program")
        raise SystemExit

import { Component, OnInit } from '@angular/core';
import { TaskModel } from '../../Shared/Models/UserModel';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserDetailsServiceService } from '../../Shared/Services/user-details-service.service';
import { DatePipe } from '@angular/common';
declare var $: any;
@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css',
})
export class UserDetailsComponent implements OnInit {
  userDetailsForm!: FormGroup;
  TaskModelList: TaskModel[] = [];
  seletedId!: number;
  isSubmited: boolean = false;
  selectDeleteId!: number;
  name!: string;
  adminName!: string;
  adminId!: number;
  taskDetails:any;
  constructor(
    private _fb: FormBuilder,
    private _userDetailService: UserDetailsServiceService,
    private _toast: ToastrService,
    private _datePipe:DatePipe
  ) {}
  ngOnInit(): void {
    let obj = {
      adminName: 'Gaurav Saini',
      adminId: 1,
    };
    localStorage.setItem('Details', JSON.stringify(obj));
    this.userDetailsForm = this.setFormState();
    this.getUserDetails();
    const d = localStorage.getItem('Details');
    if (d) {
      const a = JSON.parse(d);
      this.adminName = a.adminName;
      this.adminId = a.adminId;
    }
  }
  setFormState() {
    return this._fb.group({
      taskTitle: ['', Validators.required],
      taskStatus: ['', Validators.required],
      taskRemark: ['', Validators.required],
      duedate: ['', Validators.required],
      taskDesc: ['', Validators.required],
    });
  }
  get f(): { [key: string]: AbstractControl } {
    return this.userDetailsForm.controls;
  }
  regUser() {
    this.insertUserDetails(this.userDetailsForm);
  }
  //for accessing data
  getUserDetails() {
    this._userDetailService.GetAllTasks().subscribe(
      (res) => {
        this.TaskModelList = res as any;
      },
      (err) => {
        console.log('Error Occuring in getUserDetails()', err);
      }
    );
  }
  //for inserting new data
  insertUserDetails(userDetailsForm: any) {
    this.isSubmited = true;
    if (userDetailsForm.invalid) {
      this._toast.warning('Please fill up the input fields', 'Warning!', {
        closeButton: true,
        timeOut: 1900,
        progressBar: true,
      });
      return;
    }
    if (this.taskDetails && this.taskDetails.id>0) {
      let data: TaskModel = {
        id: this.taskDetails.id,
        taskTitle: this.userDetailsForm.value.taskTitle,
        taskStatus: this.userDetailsForm.value.taskStatus,
        taskRemarks: this.userDetailsForm.value.taskRemark,
        taskDueDate: this.userDetailsForm.value.duedate,
        taskDescription: this.userDetailsForm.value.taskDesc,
        lastUpdatedByName: this.adminName,
        lastUpdatedById: this.adminId,
        createdById:this.taskDetails.createdById,
        createdByName:this.taskDetails.createdByName,
        createdOn:new Date(),
        lastUpdatedOn:new Date()
      };
      this._userDetailService.SaveTask(data).subscribe(
        (res) => {
          this.taskDetails = null;
          this.getUserDetails();
          $('#formModal').modal('hide');
          this._toast.success('Data is successfully Updated', 'Success!', {
            closeButton: true,
            timeOut: 1900,
            progressBar: true,
          });
        },
        (_err) => {
          this._toast.error('Data is not updated', 'Error!', {
            closeButton: true,
            timeOut: 1900,
            progressBar: true,
          });
        }
      );
    } else {
      let data: TaskModel = {
        id: 0,
        taskTitle: this.userDetailsForm.value.taskTitle,
        taskStatus: this.userDetailsForm.value.taskStatus,
        taskRemarks: this.userDetailsForm.value.taskRemark,
        taskDueDate: this.userDetailsForm.value.duedate,
        taskDescription: this.userDetailsForm.value.taskDesc,
        createdByName: this.adminName,
        createdById: this.adminId,
        createdOn:new Date(),
        lastUpdatedOn: new Date(),
        lastUpdatedById:this.adminId,
        lastUpdatedByName:this.adminName,
      };
      this._userDetailService.SaveTask(data).subscribe(
        (res) => {
          this.getUserDetails();
          $('#formModal').modal('hide');
          this._toast.success('Data is successfully Inserted', 'Success!', {
            closeButton: true,
            timeOut: 1900,
            progressBar: true,
          });
        },
        (_err) => {
          this._toast.error('Data is not updated', 'Error!', {
            closeButton: true,
            timeOut: 1900,
            progressBar: true,
          });
        }
      );
    }
    this.isSubmited = false;
  }
  //for updating data
  updateStudentData(details: TaskModel) {
    this.seletedId = 1;
    if(details){
      this.taskDetails = details;
      this.userDetailsForm.get('taskTitle')?.setValue(details.taskTitle);
      this.userDetailsForm.get('duedate')?.setValue(this._datePipe.transform(details.taskDueDate,'yyyy-MM-dd'));
      this.userDetailsForm.get('taskDesc')?.setValue(details.taskDescription);
      this.userDetailsForm.get('taskRemark')?.setValue(details.taskRemarks);
      this.userDetailsForm.get('taskStatus')?.setValue(details.taskStatus);
    }
  }
  //for deleting data
  deleteUserDetail() {
    this._userDetailService.DeleteTask(this.selectDeleteId).subscribe(
      (res) => {
        this.getUserDetails();
        this._toast.success('Data is deleted', 'Success!', {
          closeButton: true,
          timeOut: 1900,
          progressBar: true,
        });
      },
      (err) => {
        this._toast.error('Data is not deleted', 'Error!', {
          closeButton: true,
          timeOut: 1900,
          progressBar: true,
        });
      }
    );
  }
  addMore() {
    this.seletedId = 0;
    this.userDetailsForm.reset();
  }
  cancel() {
    this.userDetailsForm.reset();
    this.isSubmited = false;
  }
  deleteData(data: TaskModel) {
    this.selectDeleteId = data.id;
    // this.name = data.firstName + ' ' + data.lastName;
  }
}

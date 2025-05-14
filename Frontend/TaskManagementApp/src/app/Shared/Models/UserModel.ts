export interface TaskModel {
    id: number;
    taskTitle: string;
    taskDescription: string;
    taskDueDate: string;
    taskStatus: string;
    taskRemarks: string;
    createdOn: Date;
    lastUpdatedOn: Date;
    createdByName: string;
    createdById: number;
    lastUpdatedByName: string;
    lastUpdatedById: number;
}
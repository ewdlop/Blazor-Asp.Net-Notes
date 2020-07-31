import React from 'react';
import './Task.css';

interface Task {
    title: string
}

interface TaskListProps {
    intialTasks: Task[];
}

interface TaskState {
    tasks: Task[];
}

// const TaskList: React.FC<TaskListProps> = ({ tasks }) => {
//     return (
//         <ul>
//             {tasks.map((task, i) => {
//                 return <li key={i}>{task.title}</li>
//             })}
//         </ul>
//     );
// }

const tasks = [
    { title: 'Task One' },
    { title: 'Task Two' }
];

class TaskList extends React.Component<TaskListProps, TaskState>{
    constructor(props: TaskListProps){
        super(props);
        this.state = {
            tasks: props.intialTasks
        };
        this.onAddNewTaskClick = this.onAddNewTaskClick.bind(this);
    }
    
    onAddNewTaskClick(){
        this.setState({
            tasks: [
                ...this.state.tasks,
                {title: 'New Task'}
            ]     
        });
    };

    render(){
        const { tasks } = this.state;
        return (
            <ul>
                {tasks.map((task, i) => {
                    return <li key={i}>{task.title}</li>
                })}
                <button onClick={this.onAddNewTaskClick}>Add New Task</button>
            </ul>

        );
    }
}

export default () => <div><TaskList intialTasks={tasks}></TaskList></div>;
import $ from 'jquery';
const jwt = localStorage.getItem('JWT')

const getWaitingAdmins = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: 'https://localhost:44309/api/Administrator/GetAllWaitingAdmins',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return result
    } catch (error) {
        return error.status;
    }
}

const approveAdmin = async (admin) => {
    let result;
    try {
        result = await $.ajax({
            type: "POST",
            url: 'https://localhost:44309/api/Administrator/ApproveAdmin',
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

const rejectAdmin = async (admin) => {
    let result;
    try {
        result = await $.ajax({
            type: "DELETE",
            url: 'https://localhost:44309/api/Administrator/RejectAdmin',
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

export default { getWaitingAdmins, approveAdmin, rejectAdmin }
import React from 'react'

export default function AdminDashboard() {
  return (
    <div className='m-2 text-center'>
        <h1 className='m-2 text-xl font-bold'>Admin Actions</h1>
        <div className='flex flex-wrap gap-2'>
        <button className='p-2 border rounded-md hover:bg-green-600'>Add User To Role</button>
        <button className='p-2 border rounded-md hover:bg-orange-600'>Remove User From Role</button>
        <button className='p-2 border rounded-md hover:bg-red-600'>Delete User</button>
        </div>
        </div>
  )
}
